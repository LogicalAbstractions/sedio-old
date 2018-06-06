using System;
using System.Threading.Tasks;

namespace Sedio.Server.Runtime.Execution.Queries
{
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        public Type ResultType => typeof(TResult);
        
        public async Task<object> Execute(IExecutionContext context)
        {
            return await OnExecute(context).ConfigureAwait(false);
        }

        protected abstract Task<TResult> OnExecute(IExecutionContext context);
    }
}