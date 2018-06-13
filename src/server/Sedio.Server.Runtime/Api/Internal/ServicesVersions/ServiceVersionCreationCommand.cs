using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Timing;
using Sedio.Server.Runtime.Execution;
using Sedio.Server.Runtime.Execution.Commands;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Components;

namespace Sedio.Server.Runtime.Api.Internal.ServicesVersions
{
    public sealed class ServiceVersionCreationCommand : AbstractCreationCommand<SemanticVersion,ServiceVersionInputDto,ServiceVersion>
    {
        public ServiceVersionCreationCommand(ServiceId serviceId,SemanticVersion id, ServiceVersionInputDto input) 
            : base(id, input, false)
        {
            ServiceId = serviceId;
        }
        
        public ServiceId ServiceId { get; }

        public string ServiceIdString => ServiceId.ToString();

#pragma warning disable 1998
        protected override async Task<Expression<Func<ServiceVersion, bool>>> OnGetFilterExpression(
#pragma warning restore 1998
            IExecutionContext context, SemanticVersion id)
        {
            var idString = id.ToFullString();
            
            return version => version.Version == idString;
        }

        protected override async Task OnMapToEntity(IExecutionContext context, 
                                                    SemanticVersion id, 
                                                    ServiceVersionInputDto source, 
                                                    ServiceVersion target,
                                                    bool isUpdate)
        {
            var timeProvider = context.Services.GetRequiredService<ITimeProvider>();
            
            var owningService = await context.DbContext.Services
                .Where(s => s.ServiceId == ServiceIdString)
                .FirstOrDefaultAsync(context.CancellationToken).ConfigureAwait(false);
            
            target.CacheTime = source.CacheTime;
            target.Version = source.Version.ToFullString();
            target.VersionOrder = 0;
            target.CreatedAt = timeProvider.UtcNow;
            
            target.HealthAggregation = source.HealthAggregation.ToEntity<HealthAggregationConfiguration>();
            target.HealthCheck = source.HealthCheck.ToEntity<HealthCheckConfiguration>();
            target.InstanceRetirement = source.InstanceRetirement.ToEntity<InstanceRetirementConfiguration>();
            target.InstanceRouting = source.InstanceRouting.ToEntity<InstanceRoutingConfiguration>();
            target.Notification = source.Notification.ToEntity<NotificationConfiguration>();
            target.Orchestration = source.Orchestration.ToEntity<OrchestrationConfiguration>();
        }

        protected override async Task<CreationResultType> OnExecute(IExecutionContext context)
        {
            var result = await base.OnExecute(context).ConfigureAwait(false);

            var serviceIdString = ServiceId.ToString();
            var owningService = await context.DbContext.Services.Where(s => s.ServiceId == serviceIdString)
                .Include(s => s.ServiceVersions)
                .FirstOrDefaultAsync(context.CancellationToken).ConfigureAwait(false);

            if (owningService != null)
            {
                // Load all versions of this service, and prepare an ordering for querying directly on reads.
                var orderedServices = owningService.ServiceVersions.OrderBy(c => SemanticVersion.Parse(c.Version))
                    .ToList();

                for (int i = 0; i < orderedServices.Count; ++i)
                {
                    orderedServices[i].VersionOrder = i;
                }

                await context.DbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);
                return result;
            }
            else
            {
                return result;
            }
        }
    }
}