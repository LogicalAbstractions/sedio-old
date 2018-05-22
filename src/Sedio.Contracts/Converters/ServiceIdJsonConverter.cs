using System;
using Newtonsoft.Json;
using Sedio.Contracts.Components;
using Sedio.Core.Converters;

namespace Sedio.Contracts.Converters
{
    public sealed class ServiceIdJsonConverter : StringJsonConverter<ServiceId>
    {
        protected override bool OnFromString(string value, out ServiceId result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = new ServiceId();
                return false;
            }

            result = new ServiceId(value);
            return true;
        }
    }
}