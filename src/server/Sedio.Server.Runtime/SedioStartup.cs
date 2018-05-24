using System.Linq;
using System.Net;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag.AspNetCore;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Application;
using Sedio.Server.Runtime.Http.Swagger;

namespace Sedio.Server.Runtime
{
    internal sealed class SedioStartup
    {
        public SedioStartup(IConfiguration configuration) { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwagger();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(GetType().Assembly);
        }

        public void Configure(IApplicationBuilder app)
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

            Boot(app);
        }

        private void Boot(IApplicationBuilder app)
        {
            foreach (var bootTask in app.ApplicationServices.GetServices<IStartupTask>().OrderBy(b => b.Order))
            {
                bootTask.Boot();
            }
        }
    }
}