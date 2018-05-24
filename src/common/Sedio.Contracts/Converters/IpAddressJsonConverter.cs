using System;
using System.Net;
using Newtonsoft.Json;
using Sedio.Core.Converters;

namespace Sedio.Contracts.Converters
{
    public sealed class IpAddressJsonConverter : StringJsonConverter<IPAddress>
    {
        protected override bool OnFromString(string value, out IPAddress result)
        {
            return IPAddress.TryParse(value, out result);
        }
    }
}