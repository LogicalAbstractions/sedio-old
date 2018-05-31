using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class OrchestrationConfigurationDto : AbstractProviderConfigurationDto
    {
        [JsonConstructor]
        public OrchestrationConfigurationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}