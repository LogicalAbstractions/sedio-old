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
            var branchIds = await ExecuteQuery(new BranchListQuery(pagingParameters));
            
            return Ok(branchIds);
        }
        
        [HttpGet("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The branch was not found")]
        public async Task<IActionResult> Get(string branchId)
        {
            var branchExists = await ExecuteQuery(new BranchExistsQuery(branchId));

            return branchExists ? (IActionResult) Ok(branchId) : NotFound();
        }
        
        [HttpPut("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.Created,typeof(void),Description="The branch was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest,typeof(void),Description = "Request parameters were incorrect")]
        [SwaggerResponse(HttpStatusCode.Conflict,typeof(void),Description = "The branch already exists")]
        public async Task<IActionResult> Put(string branchId)
        {
            var wasCreated = await ExecuteCommand(new BranchCreationRequest(branchId));

            if (wasCreated)
            {
                return CreatedAtAction("Get", new {branchId}, null);
            }

            return BadRequest();
        }

        [HttpDelete("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.NoContent,typeof(void),Description = "The branch was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description="The branch was not found")]
        public async Task<IActionResult> Delete(string branchId)
        {
            var wasDeleted = await ExecuteCommand(new BranchDeletionRequest(branchId));

            return wasDeleted ? (IActionResult)NoContent() : NotFound();
        }
    }
}