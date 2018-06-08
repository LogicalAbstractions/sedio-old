using Autofac;
using Sedio.Core.Timing;

namespace Sedio.Server.Runtime
{
    public sealed class SedioServerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemTimeProvider>().As<ITimeProvider>().SingleInstance();
        }
    }
}