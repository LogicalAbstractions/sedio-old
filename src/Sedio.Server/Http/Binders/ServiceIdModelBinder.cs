using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sedio.Contracts.Components;

namespace Sedio.Server.Http.Binders
{
    public sealed class ServiceIdModelBinder : AbstractStringModelBinder<ServiceId>
    {
        protected override ServiceId OnConvert(ModelBindingContext context, string value)
        {
            return new ServiceId(value);
        }
    }
}