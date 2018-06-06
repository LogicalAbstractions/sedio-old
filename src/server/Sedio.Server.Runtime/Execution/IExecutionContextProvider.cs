using System.Threading;
using System.Threading.Tasks;

namespace Sedio.Server.Runtime.Execution
{
    public interface IExecutionContextProvider
    {
        Task<IExecutionContext> GetContext(string branchId, CancellationToken cancellationToken);
    }
}