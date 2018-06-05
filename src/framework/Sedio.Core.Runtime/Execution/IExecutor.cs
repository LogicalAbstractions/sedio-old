using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses.ResultOperators;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutor<in TDbContext>
        where TDbContext : DbContext 
    {
        Task<TResult> Execute<TResult>(IExecutionContext<TDbContext> context,IExecutable<TResult> executable);
    }
}