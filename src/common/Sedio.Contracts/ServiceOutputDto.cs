using System;
using System.Collections.Generic;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceOutputDto
    {
        public ServiceOutputDto(ServiceId id, HealthAggregationConfigurationDto healthAggregation,TimeSpan? cacheTime, DateTimeOffset createdAt)
        {
            Id = id;
            HealthAggregation = healthAggregation;
            CacheTime = cacheTime;
            CreatedAt = createdAt;
        }

        public ServiceId Id { get; }

        public HealthAggregationConfigurationDto HealthAggregation { get; }

        public TimeSpan? CacheTime { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}