using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;

namespace Sedio.Server.Runtime.Model
{
    public class ServiceDependency
    {
        public class Schema : AbstractEntitySchema<ServiceDependency>
        {
            protected override void OnApply(EntityTypeBuilder<ServiceDependency> builder)
            {
                builder.HasIndex(s => s.ServiceId);
                builder.Property(s => s.ServiceId).HasMaxLength(48).IsRequired();

                builder.HasIndex(s => s.VersionRequirement);
                builder.Property(s => s.VersionRequirement).HasMaxLength(128).IsRequired();
            }
        }
        
        public long Id { get; set; }
        
        public string ServiceId { get; set; }
        
        public string VersionRequirement { get; set; }
        
        public long ServiceVersionId { get; set; }
        
        public ServiceVersion ServiceVersion { get; set; }
    }

    public static class ServiceDependencyMappingExtensions
    {
        public static ServiceDependencyDto ToOutput(this ServiceDependency serviceDependency)
        {
            if (serviceDependency == null) throw new ArgumentNullException(nameof(serviceDependency));

            return new ServiceDependencyDto()
            {
                ServiceId = serviceDependency.ServiceId,
                VersionRequirement = VersionRange.Parse(serviceDependency.VersionRequirement)
            };
        }
    }
}