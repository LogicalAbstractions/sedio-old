using System;
using System.Linq;
using System.Net;
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
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServiceInstances
{
    public sealed class ServiceInstanceCreationOrUpdateRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceInstanceCreationOrUpdateRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceInstanceCreationOrUpdateRequest request)
            {
                var dbContext = context.DbContext();

                var timeProvider = context.Services.GetRequiredService<ITimeProvider>();
                var versionString = request.ServiceVersion.ToFullString();
                
                var service = await dbContext.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId, context.CancellationToken)
                    .ConfigureAwait(false);

                if (service == null)
                {
                    return NotFound();
                }
                
                var serviceVersion =
                    await dbContext.ServiceVersions.
                        Where(s => s.Version == versionString && s.ServiceId == service.Id)
                        .FirstOrDefaultAsync(context.CancellationToken).ConfigureAwait(false);

                if (serviceVersion == null)
                {
                    return NotFound();
                }

                var addressString = request.ServiceInstanceAddress.ToString();

                var serviceInstance = await dbContext.ServiceInstances
                    .Include(i => i.ServiceVersion)
                    .Where(i => i.ServiceVersionId == serviceVersion.Id && i.Address == addressString)
                    .FirstOrDefaultAsync(context.CancellationToken)
                    .ConfigureAwait(false);

                var routeValues = new
                {
                    serviceId = request.ServiceId,
                    serviceVersion = request.ServiceVersion,
                    serviceInstanceAddress = addressString
                };

                IExecutionResponse response = null;
               
                if (serviceInstance == null)
                {
                    serviceInstance = new ServiceInstance()
                    {
                        CreatedAt = timeProvider.UtcNow,
                        Address = addressString,
                        ServiceVersion = serviceVersion
                    };

                    dbContext.ServiceInstances.Add(serviceInstance);
                    response = Created(routeValues, model: serviceInstance.ToOutput());
                }
                else
                {
                    response = Updated(routeValues, model: serviceInstance.ToOutput());
                }

                await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

                return response;
            }
        }
        
        public ServiceInstanceCreationOrUpdateRequest(string serviceId, SemanticVersion serviceVersion, IPAddress serviceInstanceAddress, ServiceInstanceInputDto input)
            : base(ExecutionRequestType.Mutation)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
            ServiceVersion = serviceVersion ?? throw new ArgumentNullException(nameof(serviceVersion));
            ServiceInstanceAddress = serviceInstanceAddress ?? throw new ArgumentNullException(nameof(serviceInstanceAddress));
            Input = input ?? throw new ArgumentNullException(nameof(input));
        }

        public string ServiceId { get; }
        
        public SemanticVersion ServiceVersion { get; }
        
        public IPAddress ServiceInstanceAddress { get; }
        
        public ServiceInstanceInputDto Input { get; }
    }
}