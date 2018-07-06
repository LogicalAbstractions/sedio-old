using System;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Branches
{
    public sealed class BranchGetRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<BranchGetRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, BranchGetRequest request)
            {
                var exists = context.DbContextManager().BranchIds.Contains(request.BranchId);

                return exists ? Ok(request.BranchId) : NotFound();
            }
        }
        
        public BranchGetRequest(string branchId)
        {
            if (string.IsNullOrWhiteSpace(branchId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(branchId));
            
            BranchId = branchId;
        }

        public string BranchId { get; }
    }
}