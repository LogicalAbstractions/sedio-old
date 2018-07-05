using Autofac;
using Sedio.Core.Timing;

namespace Sedio.Core.Runtime
{
    public sealed class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemTimeProvider>().As<ITimeProvider>().SingleInstance();
        }
    }
}