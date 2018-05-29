﻿using System;
using System.Collections.Generic;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class VersionOutputDto
    {
        public VersionOutputDto(SemanticVersion version, 
            IReadOnlyList<DependencyDto> dependencies, 
            IReadOnlyList<EndpointDto> endpoints, 
            HealthCheckDto healthCheck, 
            HealthAggregationDto healthAggregation, 
            NotificationDto notification, 
            InstanceRoutingDto instanceRouting, 
            OrchestrationDto orchestration, 
            TimeSpan? cacheTime,
            IReadOnlyDictionary<string, object> tags, 
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
            Tags = tags;
            CreatedAt = createdAt;
        }

        public SemanticVersion Version { get; }

        public IReadOnlyList<DependencyDto> Dependencies { get; }

        public IReadOnlyList<EndpointDto> Endpoints { get; }

        public HealthCheckDto HealthCheck { get; }

        public HealthAggregationDto HealthAggregation { get; }

        public NotificationDto Notification { get; }

        public InstanceRoutingDto InstanceRouting { get; }

        public OrchestrationDto Orchestration { get; }
        
        public TimeSpan? CacheTime { get; }

        public IReadOnlyDictionary<string, object> Tags { get; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}