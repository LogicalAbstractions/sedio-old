﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public sealed class HealthAggregationDto : AbstractProviderDto
    {
        [JsonConstructor]
        public HealthAggregationDto(string providerId, JObject parameters) 
            : base(providerId, parameters)
        {}
    }
}