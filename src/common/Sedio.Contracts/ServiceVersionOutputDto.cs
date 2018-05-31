using System;
using System.Collections.Generic;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceVersionOutputDto
    {
        public ServiceVersionOutputDto(SemanticVersion version, 
            IReadOnlyList<DependencyDto> dependencies, 
            IReadOnlyList<EndpointDto> endpoints, 
            HealthCheckConfigurationDto healthCheck, 
            HealthAggregationConfigurationDto healthAggregation, 
            NotificationConfigurationDto notification, 
            InstanceRoutingConfigurationDto instanceRouting, 
            OrchestrationConfigurationDto orchestration, 
            TimeSpan? cacheTime,
            DateTimeOffset createdAt)
        {
            Version = version;
            Dependencies = dependencies;
            Endpoints = endpoints;
            HealthCheck = healthCheck;
            HealthAggregation = healthAggregation;
            Notification = notification;
            InstanceRouting = instanceRouting;
            Orchestration = orchestration;
            CacheTime = cacheTime;
            CreatedAt = createdAt;
        }

        public SemanticVersion Version { get; }

        public IReadOnlyList<DependencyDto> Dependencies { get; }

        public IReadOnlyList<EndpointDto> Endpoints { get; }

        public HealthCheckConfigurationDto HealthCheck { get; }

        public HealthAggregationConfigurationDto HealthAggregation { get; }

        public NotificationConfigurationDto Notification { get; }

        public InstanceRoutingConfigurationDto InstanceRouting { get; }

        public OrchestrationConfigurationDto Orchestration { get; }
        
        public TimeSpan? CacheTime { get; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}