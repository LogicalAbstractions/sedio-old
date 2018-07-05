using Microsoft.AspNetCore.Mvc;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class DeletedExecutionResponse : AbstractExecutionResponse<DeletedExecutionResponse>
    {
        public DeletedExecutionResponse()
            : base(null)
        {
            RegisterTransform<Controller,IActionResult>((context, response) => context.Context.NoContent());
        }
    }
}