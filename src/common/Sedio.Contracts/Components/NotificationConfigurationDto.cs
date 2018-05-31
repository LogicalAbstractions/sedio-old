using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class NotificationConfigurationDto : AbstractProviderConfigurationDto
    {
        [JsonConstructor]
        public NotificationConfigurationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}