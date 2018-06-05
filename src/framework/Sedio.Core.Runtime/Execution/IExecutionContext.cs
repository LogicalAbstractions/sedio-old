using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionContext<out TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        CancellationToken CancellationToken { get; }
        
        TDbContext DbContext { get; }
    }
}