﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;
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
                
                builder.OwnsOne(s => s.StatusCheck,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));
                
                builder.OwnsOne(s => s.StatusAggregation,b => b.Property(h => h.ProviderId)
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

        public StatusCheckConfiguration StatusCheck { get; set; }
        
        public StatusAggregationConfiguration StatusAggregation { get; set; }
        
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
    
    public static class ServiceVersionMappingExtensions
    {
        public static ServiceVersionOutputDto ToOutput(this ServiceVersion serviceVersion)
        {
            if (serviceVersion == null) throw new ArgumentNullException(nameof(serviceVersion));

            return new ServiceVersionOutputDto()
            {
                Version = SemanticVersion.Parse(serviceVersion.Version),
                Dependencies = serviceVersion.ServiceDependencies.Select(d => d.ToOutput()).ToList(),
                Endpoints = serviceVersion.ServiceEndpoints.Select(e => e.ToOutput()).ToList(),
                
                StatusAggregation = serviceVersion.StatusAggregation.ToOutput<StatusAggregationConfigurationDto>(),
                StatusCheck = serviceVersion.StatusCheck.ToOutput<StatusCheckConfigurationDto>(),
                Notification = serviceVersion.Notification.ToOutput<NotificationConfigurationDto>(),
                InstanceRouting = serviceVersion.InstanceRouting.ToOutput<InstanceRoutingConfigurationDto>(),
                InstanceRetirement = serviceVersion.InstanceRetirement.ToOutput<InstanceRetirementConfigurationDto>(),
                Orchestration = serviceVersion.Orchestration.ToOutput<OrchestrationConfigurationDto>(),
                
                CacheTime = serviceVersion.CacheTime,
                CreatedAt = serviceVersion.CreatedAt
            };
        }
    }

    public static class ServiceVersionQueryExtensions
    {
        public static async Task<ServiceVersion> FindServiceVersion(this DbSet<ServiceVersion> services, Service service,SemanticVersion serviceVersion,
                                                                    CancellationToken cancellationToken,bool asNoTracking = false)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceVersion == null) throw new ArgumentNullException(nameof(serviceVersion));

            if (service == null)
            {
                return null;
            }

            var versionString = serviceVersion.ToFullString();

            var queryable = asNoTracking ? services.AsNoTracking() : services;

            return await queryable.Where(v => v.ServiceId == service.Id && v.Version == versionString)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}