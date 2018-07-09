using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public static class ServiceInstanceQueryExtensions
    {
        public static async Task<ServiceInstance> FindServiceInstance(this DbSet<ServiceInstance> serviceInstances,
                                                                      ServiceVersion serviceVersion,
                                                                      IPAddress serviceInstanceAddress,
                                                                      CancellationToken cancellationToken,
                                                                      bool asNoTracking = false)
        {
            if (serviceInstances == null) throw new ArgumentNullException(nameof(serviceInstances));
            if (serviceInstanceAddress == null) throw new ArgumentNullException(nameof(serviceInstanceAddress));
            
            if (serviceVersion == null)
            {
                return null;
            }
            
            var addressString = serviceInstanceAddress.ToString();

            var queryable = asNoTracking ? serviceInstances.AsNoTracking() : serviceInstances;

            return await queryable.Where(i => i.ServiceVersionId == serviceVersion.Id && i.Address == addressString)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}