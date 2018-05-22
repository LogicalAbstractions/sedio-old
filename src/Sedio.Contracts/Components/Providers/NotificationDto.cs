using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components.Providers
{
    public sealed class NotificationDto : AbstractProviderDto
    {
        [JsonConstructor]
        public NotificationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}