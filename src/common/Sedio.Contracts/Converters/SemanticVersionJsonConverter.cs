using System;
using Newtonsoft.Json;
using NuGet.Versioning;
using Sedio.Core.Converters;

namespace Sedio.Contracts.Converters
{
    public sealed class SemanticVersionJsonConverter : AbstractStringJsonConverter<SemanticVersion>
    {
        protected override bool OnFromString(string value, out SemanticVersion result)
        {
            return SemanticVersion.TryParse(value, out result);
        }

        protected override string OnToString(SemanticVersion value)
        {
            return value.ToFullString();
        }
    }
}
