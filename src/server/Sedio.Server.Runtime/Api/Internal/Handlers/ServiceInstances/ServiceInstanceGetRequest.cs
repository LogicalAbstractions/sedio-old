using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServiceInstances
{
    public sealed class ServiceInstanceGetRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceInstanceGetRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceInstanceGetRequest request)
            {
                var dbContext = context.DbContext();
                
                var versionString = request.ServiceVersion.ToFullString();
                
                var service = await dbContext.Services.AsNoTracking()
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
                    .AsNoTracking()    
                    .Include(i => i.ServiceVersion)
                    .Where(i => i.ServiceVersionId == serviceVersion.Id && i.Address == addressString)
                    .FirstOrDefaultAsync(context.CancellationToken)
                    .ConfigureAwait(false);

                if (serviceInstance == null)
                {
                    return NotFound();
                }

                return Ok(serviceInstance.ToOutput());
            }
        }
        
        public ServiceInstanceGetRequest(string serviceId, SemanticVersion serviceVersion, IPAddress serviceInstanceAddress)
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