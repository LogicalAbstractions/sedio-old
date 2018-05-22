using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;
using Sedio.Server.Framework.Http;

namespace Sedio.Server.Logic.Api.Http
{
    [ProducesJson]
    [Route("api/services")]
    [ApiController]
    public class ServicesController : Controller
    {
        [HttpGet]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(PagingResult<ServiceOutputDto>))]
        public async Task<IActionResult> GetMulti(PagingParameters pagingParameters)
        {
            return Ok();
        }

        [HttpGet("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(ServiceOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The service was not found")]
        public async Task<IActionResult> GetSingle(ServiceId serviceId)
        {
            return Ok();
        }

        [HttpPut("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.Accepted, typeof(void),Description = "The service was updated with new data")]
        [SwaggerResponse(HttpStatusCode.Created,typeof(void),Description="The service was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest,typeof(void),Description = "Request parameters were incorrect")]
        public async Task<IActionResult> Put(ServiceId serviceId, [FromBody]ServiceInputDto serviceDescription)
        {
            return CreatedAtAction("GetSingle", serviceDescription, new {serviceId});
        }

        [HttpDelete("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.NoContent,typeof(void),Description = "The service was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description="The service was not found")]
        public async Task<ActionResult> Delete(ServiceId serviceId)
        {
            return Ok();
        }
    }
}
hb