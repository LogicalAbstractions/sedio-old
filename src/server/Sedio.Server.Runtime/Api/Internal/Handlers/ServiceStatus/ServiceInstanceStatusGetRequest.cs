using System;
using System.Net;
using System.Threading.Tasks;
using NuGet.Versioning;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServiceStatus
{
    public sealed class ServiceInstanceStatusGetRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceInstanceStatusGetRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceInstanceStatusGetRequest request)
            {
                var dbContext = context.DbContext();

                var service = await dbContext.Services.FindService(request.ServiceId, context.CancellationToken, true)
                    .ConfigureAwait(false);

                var serviceVersion = await dbContext.ServiceVersions
                    .FindServiceVersion(service, request.ServiceVersion, context.CancellationToken, true)
                    .ConfigureAwait(false);

                var serviceInstance = await dbContext.ServiceInstances
                    .FindServiceInstance(serviceVersion, request.ServiceInstanceAddress, context.CancellationToken,
                        true).ConfigureAwait(false);

                if (serviceInstance != null)
                {
                    var serviceStatus = await dbContext.ServiceStatus
                        .FindMostRecentStatus(serviceInstance, context.CancellationToken, true).ConfigureAwait(false);

                    if (serviceStatus != null)
                    {
                        return Ok(serviceStatus.ToOutput());
                    }
                }

                return NotFound();
            }
        }
        
        public ServiceInstanceStatusGetRequest(string serviceId, SemanticVersion serviceVersion, IPAddress serviceInstanceAddress)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
            ServiceVersion = serviceVersion ?? throw new ArgumentNullException(nameof(serviceVersion));
            ServiceInstanceAddress = serviceInstanceAddress ?? throw new ArgumentNullException(nameof(serviceInstanceAddress));
        }

        public string ServiceId { get; }
        
        public SemanticVersion ServiceVersion { get; }
        
        public IPAddress ServiceInstanceAddress { get; }
    }
}