using Microsoft.AspNetCore.Mvc;
using Sedio.Core.Runtime.Http;

namespace Sedio.Server.Runtime.Http.Controllers
{
    public abstract class AbstractExecutorController : Controller
    {
        protected string BranchId => Request.GetHeaderValue("X-Branch");
    }
}