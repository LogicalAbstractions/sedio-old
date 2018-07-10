using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;
using Sedio.Server.Runtime.Model.Components;

namespace Sedio.Server.Runtime.Model
{
    public class Service
    {
        public class Schema : AbstractEntitySchema<Service>
        {
            protected override void OnApply(EntityTypeBuilder<Service> builder)
            {
                builder.Property(s => s.ServiceId).IsRequired().HasMaxLength(48);
                builder.HasIndex(s => s.ServiceId).IsUnique();
                
                builder.OwnsOne(s => s.StatusAggregation,b => b.Property(h => h.ProviderId)
                    .IsRequired()
                    .HasMaxLength(48));

                builder.HasMany(s => s.ServiceVersions)
                    .WithOne(v => v.Service)
                    .HasPrincipalKey(s => s.Id)
                    .HasForeignKey(v => v.ServiceId);
            }
        }
        
        public long Id { get; set; }
        
        public string ServiceId { get; set; }
        
        public StatusAggregationConfiguration StatusAggregation { get; set; }
        
        public TimeSpan? CacheTime { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        // Navigation properties
        
        public IList<ServiceVersion> ServiceVersions { get; set; }
    }
    
    public static class ServiceMappingExtensions
    {
        public static ServiceOutputDto ToOutput(this Service service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            return new ServiceOutputDto()
            {
                Id                = service.ServiceId,
                CacheTime         = service.CacheTime,
                CreatedAt         = service.CreatedAt,
                StatusAggregation = service.StatusAggregation.ToOutput<StatusAggregationConfigurationDto>()
            };
        }
    }

    public static class ServiceQueryExtensions
    {
        public static async Task<Service> FindService(this DbSet<Service> services, string serviceId,CancellationToken cancellationToken,bool asNoTracking = false)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            var queryable = asNoTracking ? services.AsNoTracking() : services;

            return await queryable.Where(s => s.ServiceId == serviceId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}