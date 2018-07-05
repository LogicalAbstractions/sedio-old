using System;
using Autofac;
using Sedio.Core.Runtime.Application.Assemblies;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Local;
using Sedio.Core.Runtime.Execution.Middleware;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Configurationi
{
    public sealed class ExecutionModule : Module
    {
        private readonly IAssemblyProvider assemblyProvider;

        public ExecutionModule(IAssemblyProvider assemblyProvider)
        {
            this.assemblyProvider = assemblyProvider ?? throw new ArgumentNullException(nameof(assemblyProvider));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => ExecutionMiddlewarePipeline.Compile(c.Resolve<IServiceProvider>(),
                new ModelDbContextExecutionMiddleware.Provider(),
                new ExecutionHandlerMiddleware.Provider()
            )).AsSelf().SingleInstance();
            
            builder.RegisterType<ModelDbContextExecutionMiddleware>().AsSelf().SingleInstance();
            builder.RegisterType<ExecutionHandlerMiddleware>().AsSelf().SingleInstance();

            builder.RegisterType<LocalExecutor>().As<IExecutor>().SingleInstance();

            builder.RegisterAssemblyTypes(assemblyProvider.Assemblies)
                .Where(t => t.IsAssignableTo<IExecutionRequestHandler>() && !t.IsAbstract)
                .As<IExecutionRequestHandler>().SingleInstance();
        }
    }
}