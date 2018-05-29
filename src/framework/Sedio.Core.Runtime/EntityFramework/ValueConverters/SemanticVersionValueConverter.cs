using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NuGet.Versioning;

namespace Sedio.Core.Runtime.EntityFramework.ValueConverters
{
    public sealed class SemanticVersionValueConverter : ValueConverter<SemanticVersion,string>
    {
        public SemanticVersionValueConverter() 
            : base(version => version.ToFullString(),value => SemanticVersion.Parse(value),new ConverterMappingHints(size:48))
        {
        }
    }
}