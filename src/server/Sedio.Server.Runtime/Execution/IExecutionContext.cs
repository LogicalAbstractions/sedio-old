using System;
using System.Threading;
using Sedio.Core.Runtime.EntityFramework.Management;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Execution
{
    public interface IExecutionContext : IDisposable
    {
        CancellationToken CancellationToken { get; }
        
        ModelDbContext DbContext { get; }
        
        IDbContextManager<ModelDbContext> DbContextManager { get; }
            
        IServiceProvider Services { get; }
    }
}