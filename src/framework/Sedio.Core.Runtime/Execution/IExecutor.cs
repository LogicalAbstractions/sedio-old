using System.Threading;
using System.Threading.Tasks;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutor
    {
        Task<IExecutionResponse> Execute(string branchId, IExecutionRequest request,
                                         CancellationToken cancellationToken);
    }
}