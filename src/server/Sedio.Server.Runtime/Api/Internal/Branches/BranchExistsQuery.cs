using System;
using System.Threading.Tasks;
using Sedio.Server.Runtime.Execution;
using Sedio.Server.Runtime.Execution.Queries;

namespace Sedio.Server.Runtime.Api.Internal.Branches
{
    public sealed class BranchExistsQuery : AbstractQuery<bool>
    {
        public BranchExistsQuery(string branchId)
        {
            if (string.IsNullOrWhiteSpace(branchId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(branchId));
            BranchId = branchId;
        }

        public string BranchId { get; }
        
        protected override Task<bool> OnExecute(IExecutionContext context)
        {
            return Task.FromResult(context.DbContextManager.BranchIds.Contains(BranchId));
        }
    }
}