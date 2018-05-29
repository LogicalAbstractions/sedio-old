using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Sedio.Core.Runtime.Http.Json
{
    public sealed class JsonModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonConfiguration>().As<IConfigureOptions<MvcJsonOptions>>();
        }
    }
}