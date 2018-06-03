using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;
using Sedio.Core.Runtime.EntityFramework.ValueConverters;
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
                
                builder.OwnsOne(s => s.HealthAggregation,b => b.Property(h => h.ProviderId)
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
        
        public HealthAggregationConfiguration HealthAggregation { get; set; }
        
        public TimeSpan? CacheTime { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        // Navigation properties
        
        public IList<ServiceVersion> ServiceVersions { get; set; }
    }
}