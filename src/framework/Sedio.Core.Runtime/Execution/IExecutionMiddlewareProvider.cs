using System;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionMiddlewareProvider
    {
        IExecutionMiddleware CreateInstance(IServiceProvider serviceProvider);
    }
}