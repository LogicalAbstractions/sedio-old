using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionMiddleware
    {
        Task Execute(IExecutionContext context,Func<IExecutionContext,Task> next);
    }
}