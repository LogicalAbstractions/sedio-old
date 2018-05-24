using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;

namespace Sedio.Server.Runtime.Http.Controllers
{
    [ProducesJson]
    [Route("api/services/{serviceId}/versions")]
    public class VersionsController : Controller
    {
        [HttpGet]
        [SwaggerTag("Versions")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PagingResult<VersionOutputDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The parent service was not found")]
        public async Task<IActionResult> GetMulti(ServiceId serviceId,PagingParameters pagingParameters)
        {
            return Ok();
        }

        [HttpGet("{serviceVersion}")]
        [SwaggerTag("Versions")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(VersionOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service or this version was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> GetSingle(ServiceId serviceId, SemanticVersion serviceVersion)
        {
            return Ok();
        }

        [HttpPut("{serviceVersion}")]
        [SwaggerTag("Versions")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(void),Description ="The service version was created")]
        [SwaggerResponse(HttpStatusCode.Conflict,typeof(void),Description ="A service version cannot be changed after creation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<ActionResult> Put(ServiceId serviceId, SemanticVersion serviceVersion,[FromBody]VersionInputDto versionDescription)
        {
            return CreatedAtAction("GetSingle", new {serviceId, serviceVersion});
        }
    }
}