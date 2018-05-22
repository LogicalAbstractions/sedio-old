using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components.Providers
{
    public sealed class HealthCheckDto : AbstractProviderDto
    {
        [JsonConstructor]
        public HealthCheckDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}