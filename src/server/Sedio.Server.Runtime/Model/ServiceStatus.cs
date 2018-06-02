using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;

namespace Sedio.Server.Runtime.Model
{
    public class ServiceStatus
    {
        public class Schema : AbstractEntitySchema<ServiceStatus>
        {
            protected override void OnApply(EntityTypeBuilder<ServiceStatus> builder)
            {
                builder.HasIndex(s => s.Status);
            }
        }
        
        public long Id { get; set; }
        
        public HealthStatusType Status { get; set; }
        
        public string Message { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        // Navigation properties
        
        public long ServiceInstanceId { get; set; }
        
        public ServiceInstance ServiceInstance { get; set; }
    }
}