using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sedio.Core.Runtime.Application.Dependencies;

namespace Sedio.Core.Runtime.Application
{
    public abstract class ApplicationHost<TStartupArgument> 
    {
        protected ApplicationHost(string applicationId,params Assembly[] assemblies)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(applicationId));
            
            ApplicationId = applicationId;
            Assemblies = assemblies;
        }

        public string ApplicationId { get; }
        
        public Assembly[] Assemblies { get; }
        
        public ILogger Logger { get; private set; }

        public IContainer Container { get; private set; }
             
        protected IServiceProvider BuildContainer(ILogger logger,IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            OnConfigureServices(services);
            
            var builder = new ContainerBuilder();
            
            builder.Populate(services);
            
            OnConfigureContainer(builder);

            this.Container = builder.Build();
            
            return new AutofacServiceProvider(this.Container);
        }

        protected void Start(TStartupArgument startupArgument)
        {
            OnStart(startupArgument);
        }

        protected void Stop()
        {
            OnStop();
            
            Container?.Dispose();
            Container = null;
        }
        
        protected virtual void OnStart(TStartupArgument startupArgument)
        {
            var eventListeners = Container.Resolve<IEnumerable<IApplicationEventListener>>();

            foreach (var eventListener in eventListeners.OrderByDependencies())
            {
                eventListener.OnStart();
            }
        }

        protected virtual void OnStop()
        {
            var eventListeners = Container.Resolve<IEnumerable<IApplicationEventListener>>();

            foreach (var eventListener in eventListeners.OrderByDependencies().Reverse())
            {
                eventListener.OnStop();
            }
        }

        protected virtual void OnConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(Assemblies);
        }
        
        protected virtual void OnConfigureServices(IServiceCollection services)
        {
            
        }

        protected virtual void OnConfigureLogging(IConfiguration configuration,ILoggingBuilder builder,string applicationPath)
        {
            builder.AddConsole();
        }

        protected virtual void OnConfigureConfiguration(IConfigurationBuilder builder, string applicationPath)
        {
            builder.AddJsonFile(ApplicationId.ToLowerInvariant() + ".json", false, false);
        }
    }
}