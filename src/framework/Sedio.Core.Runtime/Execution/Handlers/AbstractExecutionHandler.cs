using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using FluentValidation.Results;
using Sedio.Core.Runtime.Execution.Responses.Predefined;

namespace Sedio.Core.Runtime.Execution.Handlers
{
    public abstract class AbstractExecutionHandler<TRequest> : IExecutionRequestHandler
        where TRequest : class,IExecutionRequest
    {
        protected string DefaultIdName { get; }

        protected AbstractExecutionHandler(string defaultIdName = null)
        {
            DefaultIdName = defaultIdName;
        }
        
        public bool CanHandle(IExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return context.Request.GetType() == typeof(TRequest);
        }

        public async Task Execute(IExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var response = await OnExecute(context, (TRequest) context.Request).ConfigureAwait(false);

            if (context.Response == null)
            {
                if (response == null)
                {
                    throw new InvalidOperationException("No response provided");
                }
                
                context.Response = response;
            }
        }

        protected abstract Task<IExecutionResponse> OnExecute(IExecutionContext context, TRequest request);

        protected IExecutionResponse Ok<T>(T model,IExecutionCachingPolicy cachingPolicy = null)
        {
            return new OkExecutionResponse<T>(model,cachingPolicy);
        }

        protected IExecutionResponse Conflict()
        {
            return new ConflictExecutionResponse();
        }

        protected IExecutionResponse Deleted()
        {
            return new DeletedExecutionResponse();
        }

        protected IExecutionResponse Created(object id, string actionName,string idName = null,object model = null)
        {
            return new CreatedExecutionResponse(id,actionName)
            {
                IdName = idName ?? DefaultIdName,
                Model = model
            };
        }

        protected IExecutionResponse NotFound()
        {
            return new NotFoundExecutionResponse();
        }

        protected IExecutionResponse Updated(object id, string actionName,string idName = null,object model = null)
        {
            return new UpdatedExecutionResponse(id,actionName)
            {
                IdName = idName ?? DefaultIdName,
                Model = model
            };
        }

        protected IExecutionResponse ValidateFailed(ValidationResult validationResult = null)
        {
            return new ValidationFailedExecutionResponse(validationResult);
        }
    }
}