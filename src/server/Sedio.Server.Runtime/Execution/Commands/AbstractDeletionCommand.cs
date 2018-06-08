using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sedio.Server.Runtime.Execution.Commands
{
    public abstract class AbstractDeletionCommand<TId,TEntity> : AbstractCommand<bool> where TEntity : class
    {
        protected AbstractDeletionCommand(TId id)
        {
            Id = id;
        }

        public TId Id { get; }

        protected override async Task<bool> OnExecute(IExecutionContext context)
        {
            var dbSet              = context.DbContext.Set<TEntity>();
            var filterExpression   = await OnGetFilterExpression(context, Id).ConfigureAwait(false);
            var entity             = await dbSet.FirstOrDefaultAsync(filterExpression,context.CancellationToken).ConfigureAwait(false);

            if (entity != null)
            {
                dbSet.Remove(entity);
                await context.DbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

                return true;
            }

            return false;
        }
        
        protected abstract Task<Expression<Func<TEntity, bool>>> OnGetFilterExpression(IExecutionContext context,TId id);
    }
}