using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.EntityFramework;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServiceInstances
{
    public class ServiceInstanceListRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceInstanceListRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceInstanceListRequest request)
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

                var serviceInstances = await dbContext.ServiceInstances
                    .AsNoTracking()
                    .Include(i => i.ServiceVersion)
                    .Where(i => i.ServiceVersionId == serviceVersion.Id)
                    .ToPagedResult(request.PagingParameters, context.CancellationToken).ConfigureAwait(false);

                return Ok(serviceInstances.Map(i => i.ToOutput()));
            }
        }
        
        public ServiceInstanceListRequest(string serviceId, SemanticVersion serviceVersion, PagingParameters pagingParameters)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
            ServiceVersion = serviceVersion ?? throw new ArgumentNullException(nameof(serviceVersion));
            PagingParameters = pagingParameters ?? throw new ArgumentNullException(nameof(pagingParameters));
        }

        public string ServiceId { get; }
        
        public SemanticVersion ServiceVersion { get; }
        
        public PagingParameters PagingParameters { get; }
    }
}