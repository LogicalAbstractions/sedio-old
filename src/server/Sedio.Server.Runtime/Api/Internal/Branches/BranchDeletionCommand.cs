using System;
using System.Threading.Tasks;
using Sedio.Server.Runtime.Execution;
using Sedio.Server.Runtime.Execution.Commands;

namespace Sedio.Server.Runtime.Api.Internal.Branches
{
    public sealed class BranchDeletionCommand : AbstractCommand<bool>
    {
        public BranchDeletionCommand(string branchId)
        {
            if (string.IsNullOrWhiteSpace(branchId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(branchId));
            BranchId = branchId;
        }

        public string BranchId { get; }
        
        protected override Task<bool> OnExecute(IExecutionContext context)
        {
            return context.DbContextManager.DeleteBranch(BranchId, context.CancellationToken);
        }
    }
}