using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceVersionInputDto
    {
        [JsonConstructor]
        public ServiceVersionInputDto(SemanticVersion version,
            IReadOnlyList<DependencyDto> dependencies,
            IReadOnlyList<EndpointDto> endpoints,
            HealthCheckConfigurationDto healthCheck,
            HealthAggregationConfigurationDto healthAggregation,
            NotificationConfigurationDto notification,
            InstanceRoutingConfigurationDto instanceRouting,
            InstanceRetirementConfigurationDto instanceRetirement,
            OrchestrationConfigurationDto orchestration,
            TimeSpan? cacheTime)
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
        }

        public SemanticVersion Version { get; }

        public IReadOnlyList<DependencyDto> Dependencies { get; }

        public IReadOnlyList<EndpointDto> Endpoints { get; }

        public HealthCheckConfigurationDto HealthCheck { get; }

        public HealthAggregationConfigurationDto HealthAggregation { get; }

        public NotificationConfigurationDto Notification { get; }

        public InstanceRoutingConfigurationDto InstanceRouting { get; }

        public InstanceRetirementConfigurationDto InstanceRetirement { get; }

        public OrchestrationConfigurationDto Orchestration { get; }
        
        public TimeSpan? CacheTime { get; }
    }
}