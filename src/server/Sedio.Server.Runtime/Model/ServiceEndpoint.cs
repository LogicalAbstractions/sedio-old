using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;

namespace Sedio.Server.Runtime.Model
{
    public class ServiceEndpoint
    {
        public class Schema : AbstractEntitySchema<ServiceEndpoint>
        {
            protected override void OnApply(EntityTypeBuilder<ServiceEndpoint> builder)
            {
                builder.Property(s => s.Protocol).IsRequired().HasMaxLength(48);
            }
        }
        
        public long Id { get; set; }
        
        public string Protocol { get; set; }
        
        public int Port { get; set; }
        
        public long ServiceVersionId { get; set; }
        
        public ServiceVersion ServiceVersion { get; set; }
    }

    public static class ServiceEndpointMappingExtensions
    {
        public static ServiceEndpointDto ToOutput(this ServiceEndpoint serviceEndpoint)
        {
            if (serviceEndpoint == null) throw new ArgumentNullException(nameof(serviceEndpoint));

            return new ServiceEndpointDto()
            {
                Protocol = serviceEndpoint.Protocol,
                Port = serviceEndpoint.Port
            };
        }
    }
}