﻿using System.Threading.Tasks;

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
            var dbSet      = context.DbContext.Set<TEntity>();
            var entity     = await dbSet.FindAsync(Id, context.CancellationToken).ConfigureAwait(false);

            if (entity != null)
            {
                dbSet.Remove(entity);
                await context.DbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

                return true;
            }

            return false;
        }
    }
}