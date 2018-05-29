using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NuGet.Versioning;

namespace Sedio.Core.Runtime.EntityFramework.ValueConverters
{
    public sealed class VersionRangeValueConverter : ValueConverter<VersionRange,string>
    {
        public VersionRangeValueConverter()
            : base(range => range.ToString(), value => VersionRange.Parse(value), new ConverterMappingHints(size: 48))
        {
            
        }
    }
}