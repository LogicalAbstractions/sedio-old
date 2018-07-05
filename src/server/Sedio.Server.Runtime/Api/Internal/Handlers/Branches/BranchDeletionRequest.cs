using System;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Branches
{
    public sealed class BranchDeletionRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<BranchDeletionRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, BranchDeletionRequest request)
            {
                var wasDeleted = await context.DbContextManager()
                    .DeleteBranch(request.BranchId, context.CancellationToken).ConfigureAwait(false);

                return wasDeleted ? Deleted() : NotFound();
            }
        }
        
        public BranchDeletionRequest(string branchId)
            : base(ExecutionRequestType.Mutation)
        {
            if (string.IsNullOrWhiteSpace(branchId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(branchId));
            BranchId = branchId;
        }

        public string BranchId { get; }
    }
}