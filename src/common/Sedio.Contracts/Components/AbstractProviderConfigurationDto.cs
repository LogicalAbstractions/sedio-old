using System;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public abstract class AbstractProviderConfigurationDto : IProviderConfigurationDto
    {
        public string ProviderId { get; set; }

        public JObject ProviderParameters { get; set; }
    }
}
