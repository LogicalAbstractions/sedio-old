using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sedio.Core.Runtime.Application.Assemblies;

namespace Sedio.Core.Runtime.Application
{
    public abstract class CliApplicationHost : ApplicationHost<string[]>
    {
        protected CliApplicationHost(string applicationId, Func<IModule, bool> modulePredicate = null, params Assembly[] assemblies) 
            : base(applicationId, modulePredicate, assemblies)
        {
        }

        protected CliApplicationHost(string applicationId, IAssemblyProvider assemblyProvider, Func<IModule, bool> modulePredicate = null) 
            : base(applicationId, assemblyProvider, modulePredicate)
        {
        }

        public async Task<int> Run(string[] arguments)
        {
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));
            
            var contentRootPath = AppDomain.CurrentDomain.BaseDirectory;
            
            var bootstrapServices = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(contentRootPath);
            OnConfigureConfiguration(configurationBuilder,contentRootPath);

            var configuration = configurationBuilder.Build();

            bootstrapServices.AddLogging(builder => OnConfigureLogging(configuration, builder, contentRootPath));

            var bootstrapServiceProvider = bootstrapServices.BuildServiceProvider();
            
            BuildContainer(bootstrapServices,configuration);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (sender, args) =>
                {
                    // ReSharper disable once AccessToDisposedClosure
                    cancellationTokenSource.Cancel();
                };

                try
                {
                    Start(arguments);
                    await OnMain(arguments, cancellationTokenSource.Token);
                    return 0;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error in {ApplicationId}", ApplicationId);
                    return -1;
                }
                finally
                {
                    Stop();
                    bootstrapServiceProvider.Dispose();
                }
            }
        }

        protected abstract Task<int> OnMain(string[] arguments, CancellationToken cancellationToken);
    }
}