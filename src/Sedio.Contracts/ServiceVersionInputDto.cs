using System.Collections.Generic;
using Newtonsoft.Json;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Contracts.Components.Providers;

namespace Sedio.Contracts
{
    public sealed class ServiceVersionInputDto
    {
        [JsonConstructor]
        public ServiceVersionInputDto(SemanticVersion version, 
            IReadOnlyList<DependencyDto> dependencies, 
            IReadOnlyList<EndpointDto> endpoints, 
            HealthCheckDto healthCheck,
            HealthAggregationDto healthAggregation, 
            NotificationDto notification, 
            InstanceRoutingDto instanceRouting, 
            InstanceRetirementDto instanceRetirement,
            OrchestrationDto orchestration, 
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

        public IReadOnlyDictionary<string,object> Tags { get; }
    }
}