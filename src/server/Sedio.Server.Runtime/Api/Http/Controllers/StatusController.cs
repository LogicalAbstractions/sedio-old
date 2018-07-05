using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.Http;
using Sedio.Core.Runtime.Http.Controllers;

namespace Sedio.Server.Runtime.Api.Http.Controllers
{
    [ProducesJson]
    public class StatusController : AbstractExecutorController
    {
        [HttpGet("api/services/{serviceId}/status")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(ServiceStatusOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The service was not found")]
        public async Task<ActionResult<ServiceStatusOutputDto>> Get(string serviceId)
        {
            return Ok();
        }

        [HttpGet("api/services/{serviceId}/versions/{serviceVersion}/status")]
        [SwaggerTag("Versions")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ServiceStatusOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void),Description = "The parent service or this version was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<ActionResult<ServiceStatusOutputDto>> Get(string serviceId,SemanticVersion serviceVersion)
        {
            return Ok();
        }

        [HttpGet("api/services/{serviceId}/versions/{serviceVersion}/instances/{serviceInstanceAddress}/status")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ServiceStatusOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void),Description = "The parent service, version or this instance was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<ActionResult<ServiceStatusOutputDto>> Get(string serviceId, SemanticVersion serviceVersion,IPAddress serviceInstanceAddress)
        {
            return Ok();
        }

        [HttpPut("api/services/{serviceId}/versions/{serviceVersion}/instances/{serviceInstanceAddress}/status")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.Accepted, typeof(ServiceStatusOutputDto),Description = "The status was updated")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void),Description = "The parent service, version or this instance was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<ActionResult> Put(string serviceId, SemanticVersion serviceVersion, IPAddress serviceInstanceAddress,[FromBody]ServiceStatusInputDto statusDescription)
        {
            return Ok();
        }
    }
}