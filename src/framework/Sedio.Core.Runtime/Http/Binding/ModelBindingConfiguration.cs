using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Sedio.Core.Runtime.Http.Binding
{
    public sealed class ModelBindingConfiguration : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.AllowEmptyInputInBodyModelBinding = true;
            options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(IPAddress)));
            options.ModelBinderProviders.Insert(0, new ModelBindingProvider());
        }
    }
}