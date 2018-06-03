﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;
using Sedio.Core.Runtime.EntityFramework.ValueConverters;
using Sedio.Server.Runtime.Model.Components;

namespace Sedio.Server.Runtime.Model
{
    public class ServiceVersion
    {
        public class Schema : AbstractEntitySchema<ServiceVersion>
        {
            protected override void OnApply(EntityTypeBuilder<ServiceVersion> builder)
            {
                builder.Property(s => s.Version).IsRequired().HasMaxLength(128);
                builder.HasIndex(s => s.Version);

                builder.HasIndex(s => s.VersionOrder);
                
                builder.OwnsOne(s => s.HealthCheck,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));
                
                builder.OwnsOne(s => s.HealthAggregation,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));
                
                builder.OwnsOne(s => s.Notification,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));
                
                builder.OwnsOne(s => s.InstanceRouting,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));
                
                builder.OwnsOne(s => s.InstanceRetirement,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));
                
                builder.OwnsOne(s => s.Orchestration,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));
 
                builder.HasMany(s => s.ServiceDependencies)
                    .WithOne(d => d.ServiceVersion)
                    .HasPrincipalKey(s => s.Id)
                    .HasForeignKey(d => d.ServiceVersionId);
                
                builder.HasMany(s => s.ServiceEndpoints)
                    .WithOne(d => d.ServiceVersion)
                    .HasPrincipalKey(s => s.Id)
                    .HasForeignKey(d => d.ServiceVersionId);

                builder.HasMany(s => s.ServiceInstances)
                    .WithOne(i => i.ServiceVersion)
                    .HasPrincipalKey(s => s.Id)
                    .HasForeignKey(i => i.ServiceVersionId);
            }
        }
        
        public long Id { get; set; }
        
        public string Version { get; set; }
        
        public int VersionOrder { get; set; }

        public HealthCheckConfiguration HealthCheck { get; set; }
        
        public HealthAggregationConfiguration HealthAggregation { get; set; }
        
        public NotificationConfiguration Notification { get; set; }
        
        public InstanceRoutingConfiguration InstanceRouting { get; set; }
        
        public InstanceRetirementConfiguration InstanceRetirement { get; set; }
        
        public OrchestrationConfiguration Orchestration { get; set; }
        
        public TimeSpan? CacheTime { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        // Navigation properties
        
        public IList<ServiceDependency> ServiceDependencies { get; set; }
        
        public IList<ServiceEndpoint> ServiceEndpoints { get; set; }
        
        public IList<ServiceInstance> ServiceInstances { get; set; }
        
        public long ServiceId { get; set; }
        
        public Service Service { get; set; }
    }
}