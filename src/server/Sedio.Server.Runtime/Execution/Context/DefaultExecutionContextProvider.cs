using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sedio.Core.Runtime.EntityFramework.Management;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Execution.Context
{
    public sealed class DefaultExecutionContextProvider : IExecutionContextProvider
    {
        private readonly IServiceProvider serviceProvider;

        public DefaultExecutionContextProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IExecutionContext> GetContext(string branchId, CancellationToken cancellationToken)
        {
            var contextManager    = serviceProvider.GetRequiredService<IDbContextManager<ModelDbContext>>();
            var contextPool       = contextManager.GetPool(branchId);
            var contextHandle     = await contextPool.Aquire(cancellationToken).ConfigureAwait(false);
            
            return new DefaultExecutionContext(serviceProvider,contextHandle,cancellationToken);
        }
    }
}