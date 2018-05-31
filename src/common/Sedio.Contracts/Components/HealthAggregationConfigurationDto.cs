using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class HealthAggregationConfigurationDto : AbstractProviderConfigurationDto
    {
        [JsonConstructor]
        public HealthAggregationConfigurationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}