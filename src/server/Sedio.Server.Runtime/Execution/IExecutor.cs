using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sedio.Server.Runtime.Execution
{
    public interface IExecutor
    {
        Task<object> Execute(IExecutable executable,string branchId,CancellationToken cancellationToken);
    }

    public static class ExecutorExtensions
    {
        public static async Task<TResult> ExecuteQuery<TResult>(this IExecutor executor, IQuery<TResult> query,
            string branchId, CancellationToken cancellationToken)
        {
            if (executor == null) throw new ArgumentNullException(nameof(executor));
            if (query == null) throw new ArgumentNullException(nameof(query));

            return (TResult) await executor.Execute(query, branchId, cancellationToken).ConfigureAwait(false);
        }
        
        public static async Task<TResult> ExecuteCommand<TResult>(this IExecutor executor, ICommand<TResult> command,
            string branchId, CancellationToken cancellationToken)
        {
            if (executor == null) throw new ArgumentNullException(nameof(executor));
            if (command == null) throw new ArgumentNullException(nameof(command));

            return (TResult) await executor.Execute(command, branchId, cancellationToken).ConfigureAwait(false);
        }
    }
}