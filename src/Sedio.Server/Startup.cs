using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using NSwag.AspNetCore;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Contracts.Converters;
using Sedio.Core.Collections.Paging;
using Sedio.Server.Framework.Http.Binders;
using Sedio.Server.Framework.Swagger;

namespace Sedio.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(IPAddress)));
                options.ModelBinderProviders.Insert(0,new ContractModelBinderProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options => options.SerializerSettings.UseContractTypes());

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
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

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
