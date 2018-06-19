using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Sedio.Core.Runtime.Execution.Responses;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionResponse
    {
        IExecutionCachingPolicy CachingPolicy { get; }
        
        Task<TResult> TransformToOutput<TContext, TResult>(ExecutionResponseTransformContext<TContext> context)
            where TContext : class;
    }
}