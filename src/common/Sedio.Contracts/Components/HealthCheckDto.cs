using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class HealthCheckDto : AbstractProviderDto
    {
        [JsonConstructor]
        public HealthCheckDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}