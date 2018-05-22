using System.Collections.Generic;
using Newtonsoft.Json;
using Sedio.Contracts.Components.Providers;

namespace Sedio.Contracts
{
    public sealed class ServiceInputDto
    {
        [JsonConstructor]
        public ServiceInputDto(HealthAggregationDto healthAggregation,IReadOnlyDictionary<string, object> tags)
        {
            HealthAggregation = healthAggregation;
            Tags = tags;
        }

        public HealthAggregationDto HealthAggregation { get; }

        public IReadOnlyDictionary<string,object> Tags { get; }
    }
}