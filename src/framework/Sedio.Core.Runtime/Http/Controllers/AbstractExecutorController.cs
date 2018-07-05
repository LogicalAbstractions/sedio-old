using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Responses;

namespace Sedio.Core.Runtime.Http.Controllers
{
    public abstract class AbstractExecutorController : Controller
    {
        private readonly bool forceMainBranch;

        protected AbstractExecutorController()
        {
            forceMainBranch = false;
        }

        protected AbstractExecutorController(bool forceMainBranch)
        {
            this.forceMainBranch = forceMainBranch;
        }
        
        protected string BranchId => forceMainBranch ? null : Request.GetHeaderValue("X-Branch");

        protected async Task<IActionResult> Execute(IExecutionRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var executor = HttpContext.RequestServices.GetRequiredService<IExecutor>();
            var response = await executor.Execute(BranchId, request, HttpContext.RequestAborted);

            return await response.TransformToOutput<Controller, IActionResult>(
                new ExecutionResponseTransformContext<Controller>(BranchId, request, this,
                    this.HttpContext.RequestServices, HttpContext.RequestAborted));
        }
    }
}