using System.Threading.Tasks;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionRequestHandler
    {
        bool CanHandle(IExecutionContext context);
        
        Task Execute(IExecutionContext context);
    }
}