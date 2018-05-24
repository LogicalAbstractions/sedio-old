using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class NotificationDto : AbstractProviderDto
    {
        [JsonConstructor]
        public NotificationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}