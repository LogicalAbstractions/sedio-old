﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class InstanceRetirementDto : AbstractProviderDto
    {
        [JsonConstructor]
        public InstanceRetirementDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {
        }
    }
}