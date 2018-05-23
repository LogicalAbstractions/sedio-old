using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class OrchestrationDto : AbstractProviderDto
    {
        [JsonConstructor]
        public OrchestrationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}