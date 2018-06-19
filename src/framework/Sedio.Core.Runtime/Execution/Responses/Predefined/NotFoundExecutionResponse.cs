using Microsoft.AspNetCore.Mvc;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class NotFoundExecutionResponse : AbstractExecutionResponse<NotFoundExecutionResponse>
    { 
        public NotFoundExecutionResponse() 
            : base(null)
        {
            RegisterTransform<Controller,IActionResult>(context => context.Context.NotFound());
        }
    }
}