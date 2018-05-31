using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceInputDto
    {
        [JsonConstructor]
        public ServiceInputDto(HealthAggregationConfigurationDto healthAggregation,TimeSpan? cacheTime)
        {
            HealthAggregation = healthAggregation;
            CacheTime = cacheTime;
        }

        public HealthAggregationConfigurationDto HealthAggregation { get; }
        
        public TimeSpan? CacheTime { get; set; }
    }
}