using System;
using System.Threading.Tasks;

namespace Sedio.Server.Runtime.Execution.Commands
{
    public abstract class AbstractCommand<TResult> : ICommand<TResult>
    {
        public Type ResultType => typeof(TResult);
        
        public async Task<object> Execute(IExecutionContext context)
        {
            return await OnExecute(context).ConfigureAwait(false);
        }

        protected abstract Task<TResult> OnExecute(IExecutionContext context);
    }
}