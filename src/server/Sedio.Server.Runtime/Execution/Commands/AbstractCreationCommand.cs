using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sedio.Server.Runtime.Execution.Commands
{
    public enum CreationResultType
    {
        Created,
        Updated,
        Conflict,
        ValidationFailed,
        NotFound,
        DependencyNotFound,
    }
    
    public abstract class AbstractCreationCommand<TId,TInput,TEntity> : AbstractCommand<CreationResultType>
        where TInput : class
        where TEntity : class,new()
    { 
        private readonly bool allowUpdates;

        protected AbstractCreationCommand(TId id,TInput input,bool allowUpdates = true)
        {
            Id = id;
            Input = input ?? throw new ArgumentNullException(nameof(input));
            this.allowUpdates = allowUpdates;
        }

        public TId Id { get; }
        
        public TInput Input { get; }

        protected override async Task<CreationResultType> OnExecute(IExecutionContext context)
        {
            var dbSet = context.DbContext.Set<TEntity>();

            // Try to find an existing entity:
            var filterExpression = await OnGetFilterExpression(context, Id).ConfigureAwait(false);
            var targetEntity = await dbSet.FirstOrDefaultAsync(filterExpression,context.CancellationToken).ConfigureAwait(false);

            var isUpdate = targetEntity != null;

            if (isUpdate && !allowUpdates)
            {
                return CreationResultType.Conflict;
            }

            if (targetEntity == null)
            {
                targetEntity = new TEntity();
            }

            if (!await OnValidate(context, Id, Input, targetEntity, isUpdate).ConfigureAwait(false))
            {
                return CreationResultType.ValidationFailed;
            }

            await OnMapToEntity(context, Id, Input, targetEntity, isUpdate).ConfigureAwait(false);

            if (!isUpdate)
            {
                await dbSet.AddAsync(targetEntity, context.CancellationToken).ConfigureAwait(false);
            }

            await context.DbContext.SaveChangesAsync().ConfigureAwait(false);
            return isUpdate ? CreationResultType.Updated : CreationResultType.Created;
        }
        
        protected abstract Task<Expression<Func<TEntity, bool>>> OnGetFilterExpression(IExecutionContext context,TId id);

        protected virtual Task<bool> OnValidate(IExecutionContext context, TId id,TInput source, TEntity target,bool isUpdate)
        {
            return Task.FromResult(true);
        }

        protected abstract Task OnMapToEntity(IExecutionContext context, TId id,TInput source, TEntity target,bool isUpdate);
    }
}