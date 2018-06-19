using System;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Execution.Context;

namespace Sedio.Core.Runtime.Execution.Middleware
{
    public sealed class ExecutionMiddlewarePipeline
    {
        private readonly Func<IExecutionContext, Task> compiledPipeline;

        private ExecutionMiddlewarePipeline(Func<IExecutionContext, Task> compiledPipeline)
        {
            this.compiledPipeline = compiledPipeline ?? throw new ArgumentNullException(nameof(compiledPipeline));
        }

        public async Task Execute(IExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            await compiledPipeline.Invoke(context).ConfigureAwait(false);
        }

        public static ExecutionMiddlewarePipeline Compile(IServiceProvider serviceProvider,
                                                         params IExecutionMiddlewareProvider[] middlewareProviders)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (middlewareProviders == null) throw new ArgumentNullException(nameof(middlewareProviders));

            Func<IExecutionContext, Task> executionDelegate = context => Task.CompletedTask;

            for (var i = middlewareProviders.Length - 1; i >= 0; --i)
            {
                var localExecutionDelegate = executionDelegate;
                var middleware = middlewareProviders[i].CreateInstance(serviceProvider);

                executionDelegate = context => middleware.Execute(context, localExecutionDelegate);
            }
            
            return new ExecutionMiddlewarePipeline(executionDelegate);
        }
    }
}