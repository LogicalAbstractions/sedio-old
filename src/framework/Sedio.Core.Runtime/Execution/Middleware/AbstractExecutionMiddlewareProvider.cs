using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sedio.Core.Runtime.Execution.Middleware
{
    public abstract class AbstractExecutionMiddlewareProvider<T> : IExecutionMiddlewareProvider
        where T : IExecutionMiddleware
    {
        public IExecutionMiddleware CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
}