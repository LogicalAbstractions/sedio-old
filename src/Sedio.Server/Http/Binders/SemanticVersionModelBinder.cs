using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Versioning;

namespace Sedio.Server.Http.Binders
{
    public sealed class SemanticVersionModelBinder : AbstractStringModelBinder<SemanticVersion>
    {
        protected override SemanticVersion OnConvert(ModelBindingContext context,string value)
        {
            return SemanticVersion.Parse(value);
        }
    }
}