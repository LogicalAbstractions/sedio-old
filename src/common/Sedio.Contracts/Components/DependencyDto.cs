using Newtonsoft.Json;
using NuGet.Versioning;

namespace Sedio.Contracts.Components
{
    public sealed class DependencyDto
    {
        public ServiceId ServiceId { get; set; }

        public VersionRange VersionRequirement { get; set; }
    }
}