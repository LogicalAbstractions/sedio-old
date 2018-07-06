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

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Services
{
    public sealed class ServiceListRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceListRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceListRequest request)
            {
                var pagedServices = await context.DbContext().Services
                    .AsNoTracking()
                    .ToPagedResult(request.PagingParameters, context.CancellationToken).ConfigureAwait(false);

                return Ok(pagedServices.Map(s => s.ToOutput()));
            }
        }
        
        public ServiceListRequest(PagingParameters pagingParameters)
        {
            PagingParameters = pagingParameters ?? throw new ArgumentNullException(nameof(pagingParameters));
        }

        public PagingParameters PagingParameters { get; }
    }
}