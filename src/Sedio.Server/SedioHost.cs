using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sedio.Contracts.Converters;

namespace Sedio.Server
{
    public sealed class SedioHost : IDisposable
    {
        private readonly IWebHost webHost;

        public SedioHost(string[] arguments)
        {
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));

            webHost = WebHost.CreateDefaultBuilder(arguments)
                .ConfigureServices(services => services.AddAutofac())            
                .ConfigureAppConfiguration(CreateConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .UseStartup<SedioStartup>()
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
    }
}