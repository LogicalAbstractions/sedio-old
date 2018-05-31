using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                builder.HasKey(s => s.Id);
                builder.Property(s => s.Id).IsRequired().HasConversion<ServiceIdValueConverter>();
                builder.HasIndex(s => s.Id).IsUnique();
                
                builder.OwnsOneProviderConfiguration(s => s.HealthAggregation);

                builder.Property(s => s.CacheTime);
                builder.Property(s => s.CreatedAt).IsRequired();
            }
        }
        
        public ServiceId Id { get; set; }
        
        public HealthAggregationConfiguration HealthAggregation { get; set; }
        
        public TimeSpan? CacheTime { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
    }
}