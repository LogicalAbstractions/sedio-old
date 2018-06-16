using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sedio.Core.Runtime.Execution
{
    public sealed class LocalExecutor : IExecutor
    {
        private readonly ExecutionMiddlewarePipeline middlewarePipeline;
        private readonly IServiceProvider serviceProvider;
        
        public LocalExecutor(ExecutionMiddlewarePipeline middlewarePipeline, IServiceProvider serviceProvider)
        {
            this.middlewarePipeline = middlewarePipeline ?? throw new ArgumentNullException(nameof(middlewarePipeline));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<IExecutionResponse> Execute(string branchId, IExecutionRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var context = new DefaultExecutionContext(branchId, cancellationToken, serviceProvider)
            {
                Request = request
            };

            await middlewarePipeline.Execute(context).ConfigureAwait(false);

            return context.Response;
        }
    }
}