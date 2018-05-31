using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class InstanceRetirementConfigurationDto : AbstractProviderConfigurationDto
    {
        [JsonConstructor]
        public InstanceRetirementConfigurationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {
        }
    }
}