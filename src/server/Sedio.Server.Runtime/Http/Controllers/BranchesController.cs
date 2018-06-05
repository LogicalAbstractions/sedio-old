using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Sedio.Contracts;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Http;

namespace Sedio.Server.Runtime.Http.Controllers
{
    [ProducesJson]
    [Route("api/branches")]
    public class BranchesController : Controller
    {
        [HttpGet]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(PagingResult<string>))]
        public async Task<IActionResult> GetMulti(PagingParameters pagingParameters)
        {
            return Ok();
        }
        
        [HttpGet("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The branch was not found")]
        public async Task<IActionResult> GetSingle(string branchId)
        {
            return Ok();
        }
        
        [HttpPut("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.Created,typeof(void),Description="The branch was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest,typeof(void),Description = "Request parameters were incorrect")]
        public async Task<IActionResult> Put(string branchId)
        {
            return CreatedAtAction("GetSingle",branchId,new {branchId});
        }

        [HttpDelete("{branchId}")]
        [SwaggerTag("Branches")]
        [SwaggerResponse(HttpStatusCode.NoContent,typeof(void),Description = "The branch was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description="The branch was not found")]
        public async Task<ActionResult> Delete(string branchId)
        {
            return Ok();
        }
    }
}