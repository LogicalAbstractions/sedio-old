using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Contracts.Components;

namespace Sedio.Server.Http
{
    public class DependenciesController : Controller
    {
        [HttpGet("api/services/{serviceId}/versions/{serviceVersion}/dependencies/{dependencyId}")]
        [SwaggerTag("Versions")]
        [SwaggerResponse(HttpStatusCode.OK,typeof(VersionOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound,typeof(void),Description = "The parent service, this version or the dependency was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest,typeof(void),Description = "Request parameters were incorrect")]
        public async Task<IActionResult> GetDependencyVersion(ServiceId serviceId, SemanticVersion serviceVersion,
            ServiceId dependencyId)
        {
            return Ok();
        }

        [HttpGet("api/services/{serviceId}/versions/{serviceVersion}/instances/{serviceInstanceAddress}/dependencies/{dependencyId}")]
        [SwaggerTag("Instances")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(InstanceOutputDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "The parent service, this version or the dependency was not found")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Request parameters were incorrect")]
        public async Task<IActionResult> GetDependencyInstance(ServiceId serviceId, SemanticVersion serviceVersion,IPAddress serviceInstanceAddress,
            ServiceId dependencyId)
        {
            return Ok();
        }
    }
}