using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sedio.Core.Runtime.Application.Dependencies;

namespace Sedio.Core.Runtime.Application
{
    internal sealed class ApplicationEngine
    {
        private readonly Assembly[] assemblies;
        private readonly string applicationName;

        internal ApplicationEngine(string applicationName,params Assembly[] assemblies)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(applicationName));
            this.applicationName = applicationName;
            this.assemblies = assemblies;
        }

        internal void OnConfigureServices(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
        }

        internal void OnConfigureContainer(ContainerBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            builder.RegisterAssemblyModules(assemblies);
        }

        internal void OnStart(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null) throw new ArgumentNullException(nameof(lifetimeScope));
            
            var logger = lifetimeScope.Resolve<ILogger<ApplicationEngine>>();
            
            var applicationEventListeners =
                lifetimeScope.Resolve<IEnumerable<IApplicationEventListener>>().OrderByDependencies();

            foreach (var applicationEventListener in applicationEventListeners)
            {   
                applicationEventListener.OnStart();
            }
        }

        internal void OnStop(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null) throw new ArgumentNullException(nameof(lifetimeScope));
            
            var logger = lifetimeScope.Resolve<ILogger<ApplicationEngine>>();
            
            logger.LogInformation("Starting application");
            
            var applicationEventListeners =
                lifetimeScope.Resolve<IEnumerable<IApplicationEventListener>>().OrderByDependencies().Reverse();

            foreach (var applicationEventListener in applicationEventListeners)
            {   
                applicationEventListener.OnStop();
            }
        }
    }
}