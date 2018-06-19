using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class ValidationFailedExecutionResponse : AbstractExecutionResponse<ValidationFailedExecutionResponse>
    {
        public ValidationFailedExecutionResponse(ValidationResult validationResult = null)
            : base(null)
        {
            ValidationResult = validationResult;
            
            RegisterTransform<Controller,IActionResult>((context,response) => new BadRequestResult());
        }

        public ValidationResult ValidationResult { get; }
    }
}