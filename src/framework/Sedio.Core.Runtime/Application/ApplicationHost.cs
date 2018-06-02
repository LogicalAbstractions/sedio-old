using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sedio.Core.Runtime.Application.Assemblies;
using Sedio.Core.Runtime.Application.Dependencies;
using Sedio.Core.Runtime.Application.Modules;
using Sedio.Core.Runtime.Configuration;

namespace Sedio.Core.Runtime.Application
{
    public abstract class ApplicationHost<TStartupArgument>
    {
        private readonly Func<IModule, bool> modulePredicate;
        
        protected ApplicationHost(string applicationId, Func<IModule,bool> modulePredicate = null,params Assembly[] assemblies)
            : this(applicationId, new StaticAssemblyProvider(assemblies),modulePredicate)
        {
        }
        
        protected ApplicationHost(string applicationId,IAssemblyProvider assemblyProvider,Func<IModule,bool> modulePredicate = null)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(applicationId));
            
            ApplicationId = applicationId;
            AssemblyProvider = assemblyProvider ?? throw new ArgumentNullException(nameof(assemblyProvider));

            this.modulePredicate = modulePredicate;
        }

        public string ApplicationId { get; }
        
        public IAssemblyProvider AssemblyProvider { get; }
        
        public ILogger Logger { get; private set; }

        public IContainer Container { get; private set; }
        
        public IConfiguration Configuration { get; private set; }
             
        protected IServiceProvider BuildContainer(IServiceCollection services,ILogger logger,IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
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
            var eventListeners = Container.Resolve<IEnumerable<IApplicationService>>();

            foreach (var eventListener in eventListeners.OrderByDependencies())
            {
                eventListener.OnStart();
            }
        }

        protected virtual void OnStop()
        {
            var eventListeners = Container.Resolve<IEnumerable<IApplicationService>>();

            foreach (var eventListener in eventListeners.OrderByDependencies().Reverse())
            {
                eventListener.OnStop();
            }
        }

        protected virtual void OnConfigureContainer(ContainerBuilder builder)
        {
            var moduleRegistrar = new ModuleRegistrar(Logger, AssemblyProvider, Configuration);
            moduleRegistrar.Register(builder,modulePredicate);
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