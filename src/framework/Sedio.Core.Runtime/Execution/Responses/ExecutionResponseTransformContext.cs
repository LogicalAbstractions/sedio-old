using System;
using System.Threading;

namespace Sedio.Core.Runtime.Execution.Responses
{
    public sealed class ExecutionResponseTransformContext<TContext>
        where TContext : class
    {
        public ExecutionResponseTransformContext(string branchId, 
                                                 IExecutionRequest request, 
                                                 TContext context, 
                                                 IServiceProvider services, 
                                                 CancellationToken cancellationToken)
        {
            BranchId = branchId;
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            CancellationToken = cancellationToken;
        }

        public string BranchId { get; }
        
        public CancellationToken CancellationToken { get; }
        
        public IServiceProvider Services { get; }

        public IExecutionRequest Request { get; }

        public TContext Context { get; }
    }
}