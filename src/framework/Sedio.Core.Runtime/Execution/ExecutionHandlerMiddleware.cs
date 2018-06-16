using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sedio.Core.Runtime.Execution
{
    public sealed class ExecutionHandlerMiddleware : IExecutionMiddleware
    {
        private readonly IReadOnlyList<IExecutionRequestHandler> requestHandlers;

        public ExecutionHandlerMiddleware(IEnumerable<IExecutionRequestHandler> requestHandlers)
        {
            if (requestHandlers == null) throw new ArgumentNullException(nameof(requestHandlers));
            this.requestHandlers = requestHandlers.ToList();
        }

        public Task Execute(IExecutionContext context, Func<IExecutionContext, Task> next)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (next == null) throw new ArgumentNullException(nameof(next));

            var requestHandler = requestHandlers.FirstOrDefault(h => h.CanHandle(context));

            if (requestHandler != null)
            {
                return requestHandler.Execute(context);
            }

            return Task.FromException(new NotImplementedException($"Could not find handler for request: {context.Request}"));
        }
    }
}