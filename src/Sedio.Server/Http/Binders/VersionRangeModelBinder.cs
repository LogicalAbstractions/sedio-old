using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Versioning;

namespace Sedio.Server.Http.Binders
{
    public sealed class VersionRangeModelBinder : AbstractStringModelBinder<VersionRange>
    {
        protected override VersionRange OnConvert(ModelBindingContext context, string value)
        {
            return VersionRange.Parse(value);
        }
    }
}