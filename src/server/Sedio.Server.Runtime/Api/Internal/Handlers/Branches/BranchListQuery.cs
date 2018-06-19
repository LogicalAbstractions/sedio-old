using System;
using System.Threading.Tasks;
using Sedio.Core.Collections.Paging;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Branches
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