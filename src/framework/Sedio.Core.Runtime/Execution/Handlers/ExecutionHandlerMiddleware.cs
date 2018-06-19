using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Execution.Context;
using Sedio.Core.Runtime.Execution.Middleware;

namespace Sedio.Core.Runtime.Execution.Handlers
{
    public sealed class ExecutionHandlerMiddleware : IExecutionMiddleware
    {
        public sealed class Provider : AbstractExecutionMiddlewareProvider<ExecutionHandlerMiddleware>
        {}
        
        private readonly IReadOnlyList<IExecutionRequestHandler> requestHandlers;

        public ExecutionHandlerMiddleware(IEnumerable<IExecutionRequestHandler> requestHandlers)
        {
            if (requestHandlers == null) throw new ArgumentNullException(nameof(requestHandlers));
            this.requestHandlers = requestHandlers.ToList();
        }

        public async Task Execute(IExecutionContext context, Func<IExecutionContext, Task> next)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (next == null) throw new ArgumentNullException(nameof(next));

            var requestHandler = requestHandlers.FirstOrDefault(h => h.CanHandle(context));

            if (requestHandler != null)
            {
                await requestHandler.Execute(context).ConfigureAwait(false);
                return;
            }

            throw new NotImplementedException($"Could not find handler for request: {context.Request}");
        }
    }
}