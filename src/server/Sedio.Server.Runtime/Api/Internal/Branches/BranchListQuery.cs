using System;
using System.Threading.Tasks;
using Sedio.Core.Collections.Paging;
using Sedio.Server.Runtime.Execution;
using Sedio.Server.Runtime.Execution.Queries;

namespace Sedio.Server.Runtime.Api.Internal.Branches
{
    public sealed class BranchListQuery : AbstractQuery<PagingResult<string>>
    {
        public BranchListQuery(PagingParameters pagingParameters)
        {
            PagingParameters = pagingParameters ?? throw new ArgumentNullException(nameof(pagingParameters));
        }

        public PagingParameters PagingParameters { get; }
        
        protected override async Task<PagingResult<string>> OnExecute(IExecutionContext context)
        {
            var branchIds = context.DbContextManager.BranchIds;

            return branchIds.ToPagedResult(PagingParameters);
        }
    }
}