using System;
using Newtonsoft.Json;
using NuGet.Versioning;
using Sedio.Core.Converters;

namespace Sedio.Contracts.Converters
{
    public sealed class VersionRangeJsonConverter : StringJsonConverter<VersionRange>
    {
        protected override bool OnFromString(string value, out VersionRange result)
        {
            return VersionRange.TryParse(value, out result);
        }
    }
}
