using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NuGet.Versioning;
using Sedio.Core.Runtime.EntityFramework.Schema;
using Sedio.Core.Runtime.EntityFramework.ValueConverters;

namespace Sedio.Server.Runtime.Model
{
    public class ServiceVersion
    {
        public class Schema : AbstractEntitySchema<ServiceVersion>
        {
            protected override void OnApply(EntityTypeBuilder<ServiceVersion> builder)
            {
                builder.HasKey(s => s.Id);
                builder.Property(s => s.Id).ValueGeneratedOnAdd();
                builder.HasIndex(s => s.Id).IsUnique();

                builder.Property(s => s.Version).IsRequired().HasConversion<SemanticVersionValueConverter>();
            }
        }
        
        public int Id { get; set; }
        
        public SemanticVersion Version { get; set; }
    }
}