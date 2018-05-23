using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sedio.Server.Http.Binders
{
    public sealed class IpAddressModelBinder : AbstractStringModelBinder<IPAddress>
    {
        protected override IPAddress OnConvert(ModelBindingContext context,string value)
        {
            if (IPAddress.TryParse(value, out var address))
            {
                return address;
            }

            if (value.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                return context.HttpContext.Request.GetClientIpAddress();
            }

            throw new FormatException("Unable to parse IP Address");
        }
    }
}