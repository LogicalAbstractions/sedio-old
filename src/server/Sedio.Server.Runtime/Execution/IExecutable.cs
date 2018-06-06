using System;
using System.Threading.Tasks;

namespace Sedio.Server.Runtime.Execution
{
    public interface IExecutable
    { 
        Type ResultType { get; }

        Task<object> Execute(IExecutionContext context);
    }
}