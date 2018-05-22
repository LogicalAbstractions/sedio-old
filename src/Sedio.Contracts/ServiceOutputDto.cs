using System;
using System.Collections.Generic;
using Sedio.Contracts.Components;
using Sedio.Contracts.Components.Providers;

namespace Sedio.Contracts
{
    public sealed class ServiceOutputDto
    {
        public ServiceOutputDto(ServiceId id, HealthAggregationDto healthAggregation,IReadOnlyDictionary<string, object> tags, DateTimeOffset createdAt)
        {
            Id = id;
            HealthAggregation = healthAggregation;
            Tags = tags;
            CreatedAt = createdAt;
        }

        public ServiceId Id { get; }

        public HealthAggregationDto HealthAggregation { get; }

        public IReadOnlyDictionary<string,object> Tags { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}