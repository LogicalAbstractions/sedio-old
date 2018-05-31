using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class InstanceRoutingConfigurationDto : AbstractProviderConfigurationDto
    {
        [JsonConstructor]
        public InstanceRoutingConfigurationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}