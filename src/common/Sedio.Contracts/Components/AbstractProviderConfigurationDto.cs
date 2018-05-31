using System;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public abstract class AbstractProviderConfigurationDto
    {
        protected AbstractProviderConfigurationDto(string providerId, JObject parameters)
        {
            if (string.IsNullOrWhiteSpace(providerId))
            {
                throw new System.ArgumentException("providerId must be specified", nameof(providerId));
            }

            ProviderId = providerId;
            Parameters = parameters;
        }

        public string ProviderId { get; }

        public JObject Parameters { get; }
    }
}
