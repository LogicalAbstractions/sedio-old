using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Core.Timing;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Components;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServicesVersions
{
    public sealed class ServiceVersionCreationRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceVersionCreationRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceVersionCreationRequest request)
            {
                var dbContext = context.DbContext();
                var versionString = request.ServiceVersion.ToFullString();

                var service = await dbContext.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId,
                    context.CancellationToken)
                    .ConfigureAwait(false);

                var timeProvider = context.Services.GetRequiredService<ITimeProvider>();

                if (service == null)
                {
                    return NotFound();
                }
                
                // Check if we already have the same version:
                var existingVersion =
                    await dbContext.ServiceVersions.
                        Where(s => s.Version == versionString && s.ServiceId == service.Id)
                        .FirstOrDefaultAsync(context.CancellationToken).ConfigureAwait(false);

                if (existingVersion != null)
                {
                    return Conflict();
                }

                var newVersion = new ServiceVersion()
                {
                    CacheTime = request.Input.CacheTime,
                    CreatedAt = timeProvider.UtcNow,
                    HealthAggregation = request.Input.HealthAggregation.ToEntity<HealthAggregationConfiguration>(),
                    HealthCheck = request.Input.HealthCheck.ToEntity<HealthCheckConfiguration>(),
                    InstanceRetirement = request.Input.InstanceRetirement.ToEntity<InstanceRetirementConfiguration>(),
                    InstanceRouting = request.Input.InstanceRouting.ToEntity<InstanceRoutingConfiguration>(),
                    Notification = request.Input.Notification.ToEntity<NotificationConfiguration>(),
                    Orchestration = request.Input.Orchestration.ToEntity<OrchestrationConfiguration>(),
                    ServiceId = service.Id,
                    Service = service,
                    Version = versionString,
                    VersionOrder = -1,
                };

                var newDependencies = request.Input.Dependencies.Select(d => new ServiceDependency()
                {
                    ServiceId = service.ServiceId,
                    ServiceVersion = newVersion,
                    VersionRequirement = d.VersionRequirement.ToNormalizedString()
                }).ToList();

                var newEndpoints = request.Input.Endpoints.Select(e => new ServiceEndpoint()
                {
                    ServiceVersion = newVersion,
                    Port = e.Port,
                    Protocol = e.Protocol
                }).ToList();

                newVersion.ServiceDependencies = newDependencies;
                newVersion.ServiceEndpoints = newEndpoints;

                dbContext.ServiceVersions.Add(newVersion);

                await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);
                await ApplyOrdering(dbContext, service.Id, context.CancellationToken).ConfigureAwait(false);
                
                return Created(new
                {
                    serviceId = request.ServiceId,
                    serviceVersion = request.ServiceVersion
                }, model: newVersion.ToOutput());
            }

            private async Task ApplyOrdering(ModelDbContext context,long serviceId,CancellationToken cancellationToken)
            {
                // TODO: This has O(n) complexity in terms of service versions per service.
                // It would be nice to port semantic version handling to be executable inside the database
                var allVersions = await context.ServiceVersions.Where(s => s.ServiceId == serviceId)
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                var orderedVersions = allVersions.OrderBy(s => SemanticVersion.Parse(s.Version)).Select((v, index) =>
                {
                    v.VersionOrder = index;
                    return v;
                }).ToList();

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        
        public ServiceVersionCreationRequest(string serviceId, SemanticVersion serviceVersion, ServiceVersionInputDto input)
            : base(ExecutionRequestType.Mutation)
        {
            ServiceId = serviceId ?? throw new ArgumentNullException(nameof(serviceId));
            ServiceVersion = serviceVersion ?? throw new ArgumentNullException(nameof(serviceVersion));
            Input = input ?? throw new ArgumentNullException(nameof(input));
        }

        public string ServiceId { get; set; }
        
        public SemanticVersion ServiceVersion { get; set; }
        
        public ServiceVersionInputDto Input { get; set; }
    }
}