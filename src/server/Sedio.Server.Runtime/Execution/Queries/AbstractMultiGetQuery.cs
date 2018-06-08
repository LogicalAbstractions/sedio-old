using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Collections;

namespace Sedio.Server.Runtime.Execution.Queries
{
    public abstract class AbstractMultiGetQuery<TEntity,TOutput> : AbstractQuery<PagingResult<TOutput>>
        where TEntity : class
        where TOutput : class
    {
        protected AbstractMultiGetQuery(PagingParameters pagingParameters)
        {
            PagingParameters = pagingParameters ?? throw new ArgumentNullException(nameof(pagingParameters));
        }

        public PagingParameters PagingParameters { get; }

        protected override async Task<PagingResult<TOutput>> OnExecute(IExecutionContext context)
        {
            var filterExpression = await OnGetFilterExpression(context).ConfigureAwait(false);
            var query = context.DbContext.Set<TEntity>();
            var filteredQuery = filterExpression != null ? query.Where(filterExpression) : query;

            var result = await filteredQuery.ToPagedResult(PagingParameters,context.CancellationToken).ConfigureAwait(false);

            return result.Map(item => OnMapToOuput(context, item));
        }

        protected abstract TOutput OnMapToOuput(IExecutionContext context, TEntity entity);

        protected abstract Task<Expression<Func<TEntity, bool>>> OnGetFilterExpression(IExecutionContext context);
    }
}