using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Http;
using Sedio.Core.Runtime.Http.Controllers;
using Sedio.Server.Runtime.Api.Internal.Handlers.Branches;

namespace Sedio.Server.Runtime.Api.Http.Controllers
{
    [ProducesJson]
    [Route("api/branches")]
    public class BranchesController : AbstractExecutorController
    {
        public BranchesController() : base(true) {}
        
        [HttpGet]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(PagingResult<string>))]
        public async Task<IActionResult> GetList(PagingParameters pagingParameters)
        {
            return await Execute(new BranchListRequest(pagingParameters));
        }
        
        [HttpGet("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The branch was not found")]
        public async Task<IActionResult> Get(string branchId)
        {
            return await Execute(new BranchGetRequest(branchId));
        }
        
        [HttpPut("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.Created,typeof(string),Description="The branch was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest,typeof(void),Description = "Request parameters were incorrect")]
        [SwaggerResponse(HttpStatusCode.Conflict,typeof(void),Description = "The branch already exists")]
        public async Task<IActionResult> Put(string branchId)
        {
            return await Execute(new BranchCreationRequest(branchId));
        }

        [HttpDelete("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.NoContent,typeof(void),Description = "The branch was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description="The branch was not found")]
        public async Task<IActionResult> Delete(string branchId)
        {
            return await Execute(new BranchDeletionRequest(branchId));
        }
    }
}