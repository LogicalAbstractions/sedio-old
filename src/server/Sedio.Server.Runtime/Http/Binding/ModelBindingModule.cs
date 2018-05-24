using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Sedio.Server.Http.Binding
{
    public sealed class ModelBindingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ModelBindingConfiguration>().As<IConfigureOptions<MvcOptions>>();
        }
    }
}