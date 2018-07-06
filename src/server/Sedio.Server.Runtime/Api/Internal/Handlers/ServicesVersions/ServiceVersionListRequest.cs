using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.EntityFramework;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServicesVersions
{
    public sealed class ServiceVersionListRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceVersionListRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceVersionListRequest request)
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

                var serviceVersions = await dbContext.ServiceVersions
                    .AsNoTracking()
                    .Include(s => s.ServiceDependencies)
                    .Include(s => s.ServiceEndpoints)
                    .Where(s => s.ServiceId == service.Id)
                    .ToPagedResult(request.PagingParameters, context.CancellationToken).ConfigureAwait(false);

                return Ok(serviceVersions.Map(v => v.ToOutput()));
            }
        }
        
        public ServiceVersionListRequest(string serviceId, PagingParameters pagingParameters)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
            PagingParameters = pagingParameters ?? throw new ArgumentNullException(nameof(pagingParameters));
        }

        public string ServiceId { get; }
        
        public PagingParameters PagingParameters { get; }
    }
}