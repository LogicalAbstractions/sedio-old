using Microsoft.AspNetCore.Mvc;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class OkExecutionResponse<T> : AbstractExecutionResponse<OkExecutionResponse<T>>
    {
        public T Model { get; }
        
        public OkExecutionResponse(T model,IExecutionCachingPolicy cachingPolicy = null) 
            : base(cachingPolicy)
        {
            Model = model;
            
            this.RegisterTransform<Controller,IActionResult>((context, response) => context.Context.Ok(response.Model));
            
        }
    }
}