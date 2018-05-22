using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components.Providers
{
    public sealed class InstanceRoutingDto : AbstractProviderDto
    {
        [JsonConstructor]
        public InstanceRoutingDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}