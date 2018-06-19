using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sedio.Core.Runtime.EntityFramework.Management;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Middleware;

namespace Sedio.Server.Runtime.Model.Middleware
{
    public sealed class ModelDbContextExecutionMiddleware : IExecutionMiddleware
    {
        public sealed class Provider : AbstractExecutionMiddlewareProvider<ModelDbContextExecutionMiddleware>
        {}
        
        public const string DbContextManagerKey = "_DbContextManager";
        public const string DbContextPoolKey = "_DbContextPool";
        public const string DbContextKey = "_DbContext";
        
        public async Task Execute(IExecutionContext context, Func<IExecutionContext, Task> next)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (next == null) throw new ArgumentNullException(nameof(next));

            var dbContextManager = context.Services.GetRequiredService<IDbContextManager<ModelDbContext>>();
            var dbContextPool = dbContextManager.GetPool(context.BranchId);
            var dbContext = await dbContextPool.Aquire(context.CancellationToken).ConfigureAwait(false);

            using (dbContext)
            {
                try
                {
                    context.Items[DbContextManagerKey] = dbContextManager;
                    context.Items[DbContextPoolKey] = dbContextPool;
                    context.Items[DbContextKey] = dbContext.Value;

                    await next.Invoke(context).ConfigureAwait(false);
                }
                finally
                {
                    context.Items[DbContextKey] = null;
                    context.Items[DbContextPoolKey] = null;
                    context.Items[DbContextManagerKey] = null;
                }

            }
        }
    }

    public static class ModelDbExecutionContextExtensions
    {
        public static IDbContextManager<ModelDbContext> DbContextManager(this IExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.Items[ModelDbContextExecutionMiddleware.DbContextManagerKey] as
                IDbContextManager<ModelDbContext>;
        }
        
        
    }
}