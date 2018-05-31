using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class HealthCheckConfigurationDto : AbstractProviderConfigurationDto
    {
        [JsonConstructor]
        public HealthCheckConfigurationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}