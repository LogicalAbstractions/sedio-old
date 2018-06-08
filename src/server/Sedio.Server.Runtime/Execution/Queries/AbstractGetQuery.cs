using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sedio.Server.Runtime.Execution.Queries
{
    public abstract class AbstractGetQuery<TId,TEntity,TOutput> : AbstractQuery<TOutput>
        where TEntity : class
        where TOutput : class
    {
        protected AbstractGetQuery(TId id)
        {
            Id = id;
        }

        public TId Id { get; }
        
        protected override async Task<TOutput> OnExecute(IExecutionContext context)
        {
            var dbSet = context.DbContext.Set<TEntity>();

            var query = await OnGetFilterExpression(context,Id).ConfigureAwait(false);
            var entity = await dbSet.FirstOrDefaultAsync(query,context.CancellationToken).ConfigureAwait(false);

            if (entity != null)
            {
                return  OnMapToOutput(context, Id, entity);
            }

            return null;
        }

        protected abstract TOutput OnMapToOutput(IExecutionContext context, TId id,TEntity entity);

        protected abstract Task<Expression<Func<TEntity, bool>>> OnGetFilterExpression(IExecutionContext context,TId id);
    }
}