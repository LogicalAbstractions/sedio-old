using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using NSwag.AspNetCore;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Contracts.Converters;
using Sedio.Core.Collections.Paging;
using Sedio.Server.Framework.Http.Binders;
using Sedio.Server.Framework.Swagger;
using Sedio.Server.Logic.Api.Http;

namespace Sedio.Server
{
    public sealed class ServerHost : IDisposable
    {
        private readonly IWebHost webHost;

        public ServerHost(string[] arguments)
        {
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));

            webHost = WebHost.CreateDefaultBuilder(arguments)
                .ConfigureServices(ConfigureServices)
                .ConfigureAppConfiguration(CreateConfiguration)
                .Configure(ConfigurePipeline)
                .ConfigureLogging(ConfigureLogging)
                .UseKestrel()
                .UseSockets()
                .Build();
        }

        public Task Run(CancellationToken cancellationToken)
        {
            return webHost.RunAsync(cancellationToken);
        }

        public void Dispose()
        {
            webHost.Dispose();
        }

        private void CreateConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddJsonFile("sedio.json", false, false);
        }

        private void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder builder)
        {
            builder.AddConsole();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddAutofac();

            services.AddMvc(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                    options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(IPAddress)));
                    options.ModelBinderProviders.Insert(0, new ContractModelBinderProvider());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.UseContractTypes());

            services.AddSwagger();
        }

        private void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(GetType().Assembly);
        }

        private void ConfigurePipeline(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();

            // Enable the Swagger UI middleware and the Swagger generator
            app.UseSwaggerUi(GetType().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
                settings.GeneratorSettings.DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;

                settings.GeneratorSettings.Title = "Sedio";

                settings.GeneratorSettings.IsAspNetCore = true;

                settings.GeneratorSettings.TypeMappers.MapTo<IPAddress>()
                    .MapTo<SemanticVersion>()
                    .MapTo<ServiceId>()
                    .MapTo<VersionRange>()
                    .MapTo<PagingCursor>();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}