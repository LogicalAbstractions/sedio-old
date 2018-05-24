using Newtonsoft.Json;
using NuGet.Versioning;

namespace Sedio.Contracts.Components
{
    public sealed class DependencyDto
    {
        [JsonConstructor]
        public DependencyDto(ServiceId serviceId, VersionRange versionRequirement)
        {
            ServiceId = serviceId;
            VersionRequirement = versionRequirement;
        }

        public ServiceId ServiceId { get; }

        public VersionRange VersionRequirement { get; }
    }
}