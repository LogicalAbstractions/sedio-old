﻿using System.Net;
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
    [Route("api/services/{serviceId}/versions/{serviceVersion}/instances")]
    
    public class InstancesController : Controller
    {
        [HttpGet]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PagingResult<InstanceOutputDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service or version was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> GetMulti(ServiceId serviceId,
            SemanticVersion serviceVersion, PagingParameters pagingParameters)
        {
            return Ok();
        }

        [HttpGet("{serviceInstanceAddress}")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(InstanceOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void),Description = "The parent service, version or this instance was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> GetSingle(ServiceId serviceId,
            SemanticVersion serviceVersion, IPAddress serviceInstanceAddress)
        {
            return Ok();
        }

        [HttpPut("{serviceInstanceAddress}")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(void),Description = "The instance was created")]
        [SwaggerResponse(HttpStatusCode.Accepted,typeof(void),Description = "The instance was updated with new data")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service or version was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> Put(ServiceId serviceId, SemanticVersion serviceVersion,
            IPAddress serviceInstanceAddress, [FromBody]InstanceInputDto instanceDescription)
        {
            return CreatedAtAction("GetSingle", new {serviceId, serviceVersion, serviceInstanceAddress},instanceDescription);
        }

        [HttpDelete("{serviceInstanceAddress}")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(void),Description = "The instance was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service, version or this instance was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> Delete(ServiceId serviceId, SemanticVersion serviceVersion,IPAddress serviceInstanceAddress)
        {
            return Ok();
        }
    }
}