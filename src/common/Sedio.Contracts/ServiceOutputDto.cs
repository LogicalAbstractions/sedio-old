using System;
using System.Collections.Generic;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceOutputDto
    {
        public ServiceOutputDto(ServiceId id, HealthAggregationDto healthAggregation,TimeSpan? cacheTime,IReadOnlyDictionary<string, object> tags, DateTimeOffset createdAt)
        {
            Id = id;
            HealthAggregation = healthAggregation;
            CacheTime = cacheTime;
            Tags = tags;
            CreatedAt = createdAt;
        }

        public ServiceId Id { get; }

        public HealthAggregationDto HealthAggregation { get; }

        public TimeSpan? CacheTime { get; }
        
        public IReadOnlyDictionary<string,object> Tags { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}