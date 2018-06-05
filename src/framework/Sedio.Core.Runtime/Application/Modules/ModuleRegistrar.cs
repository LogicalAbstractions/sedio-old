using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sedio.Core.Runtime.Application.Assemblies;
using Sedio.Core.Runtime.Application.Dependencies;
using Serilog;

namespace Sedio.Core.Runtime.Application.Modules
{
    internal sealed class ModuleRegistrar
    {
        private readonly ILogger logger;
        private readonly IAssemblyProvider assemblyProvider;
        private readonly IConfiguration configuration;

        internal ModuleRegistrar(ILogger logger, IAssemblyProvider assemblyProvider,IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.assemblyProvider = assemblyProvider ?? throw new ArgumentNullException(nameof(assemblyProvider));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        internal void Register(ContainerBuilder builder,Func<IModule,bool> modulePredicate = null)
        {
            var moduleBuilder = new ContainerBuilder();

            moduleBuilder.RegisterInstance(assemblyProvider).As<IAssemblyProvider>().ExternallyOwned();
            moduleBuilder.RegisterInstance(logger).As<ILogger>().ExternallyOwned();
            moduleBuilder.RegisterInstance(configuration).As<IConfiguration>().ExternallyOwned();

            moduleBuilder.RegisterAssemblyTypes(assemblyProvider.Assemblies)
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.IsAssignableTo<IModule>())
                .As<IModule>()
                .SingleInstance();

            var finalModulePredicate = modulePredicate ?? (m => true);

            using (var moduleContainer = moduleBuilder.Build())
            {
                foreach (var module in moduleContainer.Resolve<IEnumerable<IModule>>()
                    .Where(finalModulePredicate)
                    .OrderByDependencies())
                {
                    builder.RegisterModule(module);
                }
            }
        }
    }
}