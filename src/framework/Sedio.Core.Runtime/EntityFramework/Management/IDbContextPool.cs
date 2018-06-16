using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Patterns;

namespace Sedio.Core.Runtime.EntityFramework.Management
{
    public interface IDbContextPool<T> : IDisposable
        where T : DbContext
    {
        Task<DisposableHandle<T>> Aquire(CancellationToken cancellationToken);
    }
}