using Microsoft.AspNetCore.Mvc;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class ConflictExecutionResponse : AbstractExecutionResponse<ConflictExecutionResponse>
    {
        public ConflictExecutionResponse() 
            : base(null)
        {
            RegisterTransform<Controller,IActionResult>((context, response) => context.Context.Conflict());
        }
    }
}