using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        Task<IExecutionContext<TDbContext>> GetContext(string branchId, CancellationToken cancellationToken);
    }
}