using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sedio.Core.Runtime.Application.Assemblies;

namespace Sedio.Core.Runtime.Application
{
    public abstract class WebApplicationHost : ApplicationHost<IApplicationBuilder>
    {
        internal sealed class WebApplicationStartup
        {
            private readonly WebApplicationHost host;
            private readonly IConfiguration configuration;
            
            public WebApplicationStartup(IConfiguration configuration,WebApplicationHost host)
            {           
                this.host = host ?? throw new ArgumentNullException(nameof(host));
                this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            }
            
            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                if (services == null) throw new ArgumentNullException(nameof(services));

                return host.BuildContainer(services,configuration);
            }

            public void Configure(IApplicationBuilder applicationBuilder)
            {
                applicationBuilder.ApplicationServices.GetRequiredService<IApplicationLifetime>()
                    .ApplicationStopped.Register(_ => host.Stop(),null);
                
                host.Start(applicationBuilder);
            }
        }

        protected WebApplicationHost(string applicationId, Func<IModule, bool> modulePredicate = null, params Assembly[] assemblies) 
            : base(applicationId, modulePredicate, assemblies)
        {
        }

        protected WebApplicationHost(string applicationId, IAssemblyProvider assemblyProvider, Func<IModule, bool> modulePredicate = null) 
            : base(applicationId, assemblyProvider, modulePredicate)
        {
        }

        public async Task<int> Run(string[] arguments,CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var webHost = BuildWebHost(arguments))
            {
                await webHost.RunAsync(cancellationToken);
                return 0;
            }
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            var mvcBuilder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                
            foreach (var assembly in AssemblyProvider.Assemblies)
            {
                mvcBuilder.AddApplicationPart(assembly);
            }
        }

        private IWebHost BuildWebHost(string[] arguments)
        {
            return WebHost.CreateDefaultBuilder(arguments)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(this);
                })            
                .ConfigureAppConfiguration((context, builder) => 
                    OnConfigureConfiguration(builder,context.HostingEnvironment.ContentRootPath))
                .ConfigureLogging((context, builder) => 
                    OnConfigureLogging(context.Configuration,builder,context.HostingEnvironment.ContentRootPath))
                .UseStartup<WebApplicationStartup>()
                .UseKestrel((context, options) =>
                {
                    
                } )
                .UseSockets(options =>
                {
                   
                })
                .Build();
        }     
    }
}