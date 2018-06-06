using System;
using System.Threading.Tasks;
using Sedio.Server.Runtime.Execution;
using Sedio.Server.Runtime.Execution.Commands;

namespace Sedio.Server.Runtime.Api.Internal.Branches
{
    public sealed class BranchCreationCommand : AbstractCommand<bool>
    {
        public BranchCreationCommand(string branchId)
        {
            if (string.IsNullOrWhiteSpace(branchId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(branchId));
            BranchId = branchId;
        }

        public string BranchId { get; }
        
        protected override async Task<bool> OnExecute(IExecutionContext context)
        {
            return await context.DbContextManager.CreateBranch(null, BranchId, context.CancellationToken).ConfigureAwait(false);
        }
    }
}