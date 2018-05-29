﻿using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using NSwag.AspNetCore;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Runtime.Application;
using Sedio.Core.Runtime.Http.Swagger;

namespace Sedio.Server.Runtime
{
    public sealed class SedioServerHost : WebApplicationHost
    {
        public SedioServerHost() 
            : base("Sedio.Server", typeof(SedioServerHost).Assembly)
        {
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);
            services.AddSwagger();
        }

        protected override void OnStart(IApplicationBuilder app)
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
            
            base.OnStart(app);
        }
    }
}