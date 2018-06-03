using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sedio.Core.Runtime.EntityFramework.Management
{
    public interface IDbContextPool<T> : IDisposable
        where T : DbContext
    {
        Task<IDbContextHandle<T>> Aquire(CancellationToken cancellationToken);
    }
}