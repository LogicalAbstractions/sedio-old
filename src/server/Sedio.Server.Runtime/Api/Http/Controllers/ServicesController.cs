using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Http;
using Sedio.Core.Runtime.Http.Controllers;
using Sedio.Server.Runtime.Api.Internal.Handlers.Services;
using Sedio.Server.Runtime.Execution.Commands;

namespace Sedio.Server.Runtime.Api.Http.Controllers
{
    [ProducesJson]
    [Route("api/services")]
    public class ServicesController : AbstractExecutorController
    {
        [HttpGet]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(PagingResult<ServiceOutputDto>))]
        public async Task<IActionResult> GetList(PagingParameters pagingParameters)
        {
            var result = await ExecuteQuery(new ServiceListQuery(pagingParameters));

            return Ok(result);
        }

        [HttpGet("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(ServiceOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The service was not found")]
        public async Task<IActionResult> Get(ServiceId serviceId)
        {
            var result = await ExecuteQuery(new ServiceGetQuery(serviceId.ToString()));

            return result != null ? (IActionResult) Ok(result) : NotFound();
        }

        [HttpPut("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.Accepted, typeof(void),Description = "The service was updated with new data")]
        [SwaggerResponse(HttpStatusCode.Created,typeof(void),Description="The service was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest,typeof(void),Description = "Request parameters were incorrect")]
        public async Task<IActionResult> Put(ServiceId serviceId, [FromBody]ServiceInputDto serviceDescription)
        {
            var result = await ExecuteCommand(new ServiceCreationCommand(serviceId.ToString(), serviceDescription));

            if (result == CreationResultType.Created)
            {
                return CreatedAtAction("Get",new {serviceId},null);
            }
            else
            {
                return result.ToHttpStatusResult();
            }
        }

        [HttpDelete("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.NoContent,typeof(void),Description = "The service was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description="The service was not found")]
        public async Task<IActionResult> Delete(ServiceId serviceId)
        {
            var wasDeleted = await ExecuteCommand(new ServiceDeletionCommand(serviceId.ToString()));

            return wasDeleted ? (IActionResult) NoContent() : NotFound();
        }
    }
}
