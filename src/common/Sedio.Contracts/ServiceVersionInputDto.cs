using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceVersionInputDto
    {
        public IReadOnlyList<ServiceDependencyDto> Dependencies { get; set; }

        public IReadOnlyList<ServiceEndpointDto> Endpoints { get; set; }

        public StatusCheckConfigurationDto StatusCheck { get; set; }

        public StatusAggregationConfigurationDto StatusAggregation { get; set; }

        public NotificationConfigurationDto Notification { get; set; }

        public InstanceRoutingConfigurationDto InstanceRouting { get; set; }

        public InstanceRetirementConfigurationDto InstanceRetirement { get; set; }

        public OrchestrationConfigurationDto Orchestration { get; set; }
        
        public TimeSpan? CacheTime { get; set; }
    }
}