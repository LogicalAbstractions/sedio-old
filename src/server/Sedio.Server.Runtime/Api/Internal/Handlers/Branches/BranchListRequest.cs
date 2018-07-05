using System;
using System.Threading.Tasks;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Branches
{
    public sealed class BranchListRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<BranchListRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, BranchListRequest request)
            {
                var result = context.DbContextManager().BranchIds.ToPagedResult(request.PagingParameters);
                return Ok(result);
            }
        }
        
        public BranchListRequest(PagingParameters pagingParameters)
        {
            PagingParameters = pagingParameters ?? throw new ArgumentNullException(nameof(pagingParameters));
        }

        public PagingParameters PagingParameters { get; }

    }
}