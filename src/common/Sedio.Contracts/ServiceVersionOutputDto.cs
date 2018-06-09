using System;
using System.Collections.Generic;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceVersionOutputDto
    {
        public SemanticVersion Version { get; set; }

        public IReadOnlyList<DependencyDto> Dependencies { get; set; }

        public IReadOnlyList<EndpointDto> Endpoints { get; set; }

        public HealthCheckConfigurationDto HealthCheck { get; set; }

        public HealthAggregationConfigurationDto HealthAggregation { get; set; }

        public NotificationConfigurationDto Notification { get; set; }

        public InstanceRoutingConfigurationDto InstanceRouting { get; set; }
        
        public InstanceRetirementConfigurationDto InstanceRetirement { get; set; }

        public OrchestrationConfigurationDto Orchestration { get; set; }
        
        public TimeSpan? CacheTime { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}