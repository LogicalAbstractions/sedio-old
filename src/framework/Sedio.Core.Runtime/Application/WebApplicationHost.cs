using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sedio.Core.Runtime.Application
{
    public abstract class WebApplicationHost : ApplicationHost<IApplicationBuilder>
    {
        internal sealed class WebApplicationStartup
        {
            private readonly ILogger logger;
            private readonly WebApplicationHost host;
            
            public WebApplicationStartup(ILoggerFactory loggerFactory, WebApplicationHost host)
            {
                if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
                if (host == null) throw new ArgumentNullException(nameof(host));
                
                this.logger = loggerFactory.CreateLogger(host.ApplicationId);
                this.host = host;
            }
            
            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                if (services == null) throw new ArgumentNullException(nameof(services));

                return host.BuildContainer(services);
            }

            public void Configure(IApplicationBuilder applicationBuilder)
            {
                applicationBuilder.ApplicationServices.GetRequiredService<IApplicationLifetime>()
                    .ApplicationStopped.Register(_ => host.Stop(),null);
                
                host.Start(applicationBuilder);
            }
        }

        protected WebApplicationHost(string applicationId, params Assembly[] assemblies) 
            : base(applicationId, assemblies)
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

            var mvcBuilder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices()
                .AddViewComponentsAsServices()
                .AddTagHelpersAsServices();

            foreach (var assembly in Assemblies)
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