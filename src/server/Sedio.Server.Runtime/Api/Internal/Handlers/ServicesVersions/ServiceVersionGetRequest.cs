using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServicesVersions
{
    public sealed class ServiceVersionGetRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceVersionGetRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceVersionGetRequest request)
            {
                var dbContext = context.DbContext();

                var service = await dbContext.Services
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId, context.CancellationToken)
                    .ConfigureAwait(false);

                if (service == null)
                {
                    return NotFound();
                }

                var versionString = request.ServiceVersion.ToFullString();
                
                var serviceVersion = await dbContext.ServiceVersions
                    .AsNoTracking()
                    .Include(s => s.ServiceDependencies)
                    .Include(s => s.ServiceEndpoints)
                    .FirstOrDefaultAsync(v => v.ServiceId == service.Id && v.Version == versionString,
                        context.CancellationToken).ConfigureAwait(false);

                return serviceVersion != null ? Ok(serviceVersion.ToOutput()) : NotFound();
            }
        }
        
        public string ServiceId { get; }
        
        public SemanticVersion ServiceVersion { get; }
        
        public ServiceVersionGetRequest(string serviceId, SemanticVersion serviceVersion)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
            ServiceVersion = serviceVersion ?? throw new ArgumentNullException(nameof(serviceVersion));
        }
    }
}