using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Sedio.Core.Runtime.Execution;

namespace Sedio.Core.Runtime.Http.Filters
{
    public sealed class ExecutionContextFilter<TDbContext> : IAsyncActionFilter
        where TDbContext : DbContext
    {
        private readonly IExecutionContextProvider<TDbContext> executionContextProvider;

        public ExecutionContextFilter(IExecutionContextProvider<TDbContext> executionContextProvider)
        {
            this.executionContextProvider = executionContextProvider ?? throw new ArgumentNullException(nameof(executionContextProvider));
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executionContextParameter =
                context.ActionDescriptor.Parameters.FirstOrDefault(p =>
                    p.ParameterType == typeof(IExecutionContext<TDbContext>));
            
            if (executionContextParameter != null)
            {
                string branchId = null;

                if (context.HttpContext.Request.TryGetHeaderValueAs<string>("X-Branch", out var branchIdString))
                {
                    branchId = branchIdString;
                    
                    using (var executionContext =
                        await executionContextProvider.GetContext(branchId, context.HttpContext.RequestAborted))
                    {
                        context.ActionArguments[executionContextParameter.Name] = executionContext;

                        await next.Invoke();
                    }
                }
            }
            else
            {
                await next.Invoke();
            }    
        }
    }
}