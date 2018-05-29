using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceInputDto
    {
        [JsonConstructor]
        public ServiceInputDto(HealthAggregationDto healthAggregation,TimeSpan? cacheTime,IReadOnlyDictionary<string, object> tags)
        {
            HealthAggregation = healthAggregation;
            CacheTime = cacheTime;
            Tags = tags;
        }

        public HealthAggregationDto HealthAggregation { get; }
        
        public TimeSpan? CacheTime { get; }

        public IReadOnlyDictionary<string,object> Tags { get; }
    }
}