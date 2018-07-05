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
            return await Execute(new ServiceListRequest(pagingParameters));
        }

        [HttpGet("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(ServiceOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The service was not found")]
        public async Task<IActionResult> Get(string serviceId)
        {
            return await Execute(new ServiceGetRequest(serviceId));
        }

        [HttpPut("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.Accepted, typeof(ServiceOutputDto),Description = "The service was updated with new data")]
        [SwaggerResponse(HttpStatusCode.Created,typeof(ServiceOutputDto),Description="The service was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest,typeof(void),Description = "Request parameters were incorrect")]
        public async Task<IActionResult> Put(string serviceId, [FromBody]ServiceInputDto serviceDescription)
        {
            return await Execute(new ServiceCreationOrUpdateRequest(serviceId, serviceDescription));
        }

        [HttpDelete("{serviceId}")]
        [SwaggerTag("Services")]
        [SwaggerResponse(HttpStatusCode.NoContent,typeof(void),Description = "The service was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description="The service was not found")]
        public async Task<IActionResult> Delete(string serviceId)
        {
            return await Execute(new ServiceDeletionRequest(serviceId));
        }
    }
}
