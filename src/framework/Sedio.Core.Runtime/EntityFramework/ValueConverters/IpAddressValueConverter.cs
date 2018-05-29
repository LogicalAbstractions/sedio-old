using System.Net;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sedio.Core.Runtime.EntityFramework.ValueConverters
{
    public sealed class IpAddressValueConverter : ValueConverter<IPAddress,string>
    {
        public IpAddressValueConverter()
            : base(address => address.ToString(),value => IPAddress.Parse(value),new ConverterMappingHints(size: 45)) 
        {}
    }
}