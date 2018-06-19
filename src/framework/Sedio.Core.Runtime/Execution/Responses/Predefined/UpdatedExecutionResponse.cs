using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class UpdatedExecutionResponse : AbstractExecutionResponse<UpdatedExecutionResponse>
    {
        public UpdatedExecutionResponse() 
            : base(null)
        {
            RegisterTransform<Controller,IActionResult>((context, response) => context.Context.StatusCode((int)HttpStatusCode.Accepted) );
        }
    }
}