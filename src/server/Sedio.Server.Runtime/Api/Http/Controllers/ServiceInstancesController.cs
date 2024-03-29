﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Http;
using Sedio.Core.Runtime.Http.Controllers;
using Sedio.Server.Runtime.Api.Internal.Handlers.ServiceInstances;

namespace Sedio.Server.Runtime.Api.Http.Controllers
{
    [ProducesJson]
    [Route("api/services/{serviceId}/versions/{serviceVersion}/instances")]
    
    public class ServiceInstancesController : AbstractExecutorController
    {
        [HttpGet]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PagingResult<ServiceInstanceOutputDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service or version was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> GetList(string serviceId,
            SemanticVersion serviceVersion, PagingParameters pagingParameters)
        {
            return await Execute(new ServiceInstanceListRequest(serviceId, serviceVersion, pagingParameters));
        }

        [HttpGet("{serviceInstanceAddress}")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ServiceInstanceOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void),Description = "The parent service, version or this instance was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> Get(string serviceId,
            SemanticVersion serviceVersion, IPAddress serviceInstanceAddress)
        {
            return await Execute(new ServiceInstanceGetRequest(serviceId, serviceVersion, serviceInstanceAddress));
        }

        [HttpPut("{serviceInstanceAddress}")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(ServiceInstanceOutputDto),Description = "The instance was created")]
        [SwaggerResponse(HttpStatusCode.Accepted,typeof(ServiceInstanceOutputDto),Description = "The instance was updated with new data")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service or version was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> Put(string serviceId, SemanticVersion serviceVersion,
            IPAddress serviceInstanceAddress, [FromBody]ServiceInstanceInputDto serviceInstanceDescription)
        {
            return await Execute(new ServiceInstanceCreationOrUpdateRequest(serviceId, serviceVersion,
                serviceInstanceAddress, serviceInstanceDescription));
        }

        [HttpDelete("{serviceInstanceAddress}")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(void),Description = "The instance was deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service, version or this instance was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]

        public async Task<IActionResult> Delete(string serviceId, SemanticVersion serviceVersion,IPAddress serviceInstanceAddress)
        {
            return await Execute(new ServiceInstanceDeletionRequest(serviceId, serviceVersion, serviceInstanceAddress));
        }
    }
}