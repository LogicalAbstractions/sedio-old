using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sedio.Core.Runtime.Http;
using Sedio.Server.Runtime.Execution;

namespace Sedio.Server.Runtime.Api.Http.Controllers
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

        protected Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            
            var executor = HttpContext.RequestServices.GetRequiredService<IExecutor>();

            return executor.ExecuteQuery(query, BranchId, HttpContext.RequestAborted);
        }

        protected Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            
            var executor = HttpContext.RequestServices.GetRequiredService<IExecutor>();

            return executor.ExecuteCommand(command, BranchId, HttpContext.RequestAborted);
        }
    }
}