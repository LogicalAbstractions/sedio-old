using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sedio.Server.Runtime.Execution
{
    public interface IExecutable
    { 
        Type ResultType { get; }

        object Execute(IExecutionContext context);
    }

    public interface IQuery : IExecutable
    {
        
    }

    public interface IQuery<TResult> : IQuery
    {
        
    }

    public interface ICommand : IExecutable
    {
        
    }

    public interface ICommand<TResult> : ICommand
    {
        
    }
    
    public interface IExecutor
    {
        Task<object> Execute(IExecutable executable, string branchId,CancellationToken cancellationToken);
    }

    public static class ExecutorExtensions
    {
        
    }
}