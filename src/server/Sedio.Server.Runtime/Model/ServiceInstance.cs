using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Core.Runtime.EntityFramework.Schema;

namespace Sedio.Server.Runtime.Model
{
    public class ServiceInstance
    {
        public class Schema : AbstractEntitySchema<ServiceInstance>
        {
            protected override void OnApply(EntityTypeBuilder<ServiceInstance> builder)
            {
                builder.HasIndex(s => s.Address);
                builder.Property(s => s.Address).IsRequired().HasMaxLength(48);
                
                builder.HasMany(i => i.ServiceStatus)
                    .WithOne(s => s.ServiceInstance)
                    .HasPrincipalKey(i => i.Id)
                    .HasForeignKey(s => s.ServiceInstanceId);
            }
        }
        
        public long Id { get; set; }
        
        public string Address { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        // Navigation
        
        public IList<ServiceStatus> ServiceStatus { get; set; }
        
        public long ServiceVersionId { get; set; }
        
        public ServiceVersion ServiceVersion { get; set; }
    }
    
    public static class ServiceInstanceMappingExtensions
    {
        public static ServiceInstanceOutputDto ToOutput(this ServiceInstance serviceInstance)
        {
            if (serviceInstance == null) throw new ArgumentNullException(nameof(serviceInstance));

            return new ServiceInstanceOutputDto()
            {
                Address = IPAddress.Parse(serviceInstance.Address),
                CreatedAt = serviceInstance.CreatedAt,
                Version = SemanticVersion.Parse(serviceInstance.ServiceVersion.Version)
            };
        }
    }
}