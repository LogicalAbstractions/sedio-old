using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Sedio.Core.Runtime.EntityFramework.Management;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Execution.Context
{
    public sealed class DefaultExecutionContext : IExecutionContext
    {
        private readonly IDbContextHandle<ModelDbContext> dbContextHandle;
        
        internal DefaultExecutionContext(IServiceProvider services,IDbContextHandle<ModelDbContext> dbContextHandle, CancellationToken cancellationToken)
        {
            this.dbContextHandle = dbContextHandle ?? throw new ArgumentNullException(nameof(dbContextHandle));
            
            Services = services ?? throw new ArgumentNullException(nameof(services));
            CancellationToken = cancellationToken;
        }

        public CancellationToken CancellationToken { get; }

        public ModelDbContext DbContext => dbContextHandle.Context;

        public IDbContextManager<ModelDbContext> DbContextManager =>
            Services.GetRequiredService<IDbContextManager<ModelDbContext>>();
        
        public IServiceProvider Services { get; }
        
        public void Dispose()
        {
            dbContextHandle.Dispose();
        }
    }
}