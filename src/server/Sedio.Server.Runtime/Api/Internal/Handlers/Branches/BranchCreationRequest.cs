using System;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Branches
{
    public sealed class BranchCreationRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<BranchCreationRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, BranchCreationRequest request)
            {
                var wasCreated = await context.DbContextManager()
                    .CreateBranch(null, request.BranchId, context.CancellationToken)
                    .ConfigureAwait(false);

                return wasCreated ? Created(new
                {
                    branchId = request.BranchId
                },model: request.BranchId) : Conflict();
            }
        }
        
        public BranchCreationRequest(string branchId) 
            : base(ExecutionRequestType.Mutation)
        {
            if (string.IsNullOrWhiteSpace(branchId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(branchId));
            
            BranchId = branchId;
        }

        public string BranchId { get; }
    }
}