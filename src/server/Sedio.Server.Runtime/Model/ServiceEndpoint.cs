using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
}