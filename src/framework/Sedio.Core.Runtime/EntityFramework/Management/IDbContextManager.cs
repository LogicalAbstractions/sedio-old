using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;

namespace Sedio.Core.Runtime.EntityFramework.Management
{
    public interface IDbContextManager<T>
        where T : DbContext
    {
        void Initialize();

        ISet<string> BranchIds { get; }
        
        Task CreateBranch(string sourceId,string targetId,CancellationToken cancellationToken);

        Task DeleteBranch(string id,CancellationToken cancellationToken);

        Task<IDbContextHandle<T>> Aquire(string id,CancellationToken cancellationToken);
    }
}