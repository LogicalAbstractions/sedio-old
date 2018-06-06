using Autofac;
using Sedio.Server.Runtime.Execution.Context;
using Sedio.Server.Runtime.Execution.Local;

namespace Sedio.Server.Runtime.Execution
{
    public sealed class ExecutionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalExecutor>().As<IExecutor>().SingleInstance();
            builder.RegisterType<DefaultExecutionContextProvider>().As<IExecutionContextProvider>().SingleInstance();
        }
    }
}