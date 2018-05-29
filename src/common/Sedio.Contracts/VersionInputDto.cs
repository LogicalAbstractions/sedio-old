using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class VersionInputDto
    {
        [JsonConstructor]
        public VersionInputDto(SemanticVersion version, 
            IReadOnlyList<DependencyDto> dependencies, 
            IReadOnlyList<EndpointDto> endpoints, 
            HealthCheckDto healthCheck,
            HealthAggregationDto healthAggregation, 
            NotificationDto notification, 
            InstanceRoutingDto instanceRouting, 
            InstanceRetirementDto instanceRetirement,
            OrchestrationDto orchestration, 
            TimeSpan? cacheTime,
            IReadOnlyDictionary<string, object> tags)
        {
            Version = version;
            Dependencies = dependencies;
            Endpoints = endpoints;
            HealthCheck = healthCheck;
            HealthAggregation = healthAggregation;
            Notification = notification;
            InstanceRouting = instanceRouting;
            InstanceRetirement = instanceRetirement;
            Orchestration = orchestration;
            CacheTime = cacheTime;
            Tags = tags;
        }

        public SemanticVersion Version { get; }

        public IReadOnlyList<DependencyDto> Dependencies { get; }

        public IReadOnlyList<EndpointDto> Endpoints { get; }

        public HealthCheckDto HealthCheck { get; }

        public HealthAggregationDto HealthAggregation { get; }

        public NotificationDto Notification { get; }

        public InstanceRoutingDto InstanceRouting { get; }

        public InstanceRetirementDto InstanceRetirement { get; }

        public OrchestrationDto Orchestration { get; }
        
        public TimeSpan? CacheTime { get; }

        public IReadOnlyDictionary<string,object> Tags { get; }
    }
}