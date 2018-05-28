using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sedio.Core.Runtime.Application
{
    public abstract class CliApplicationHost : ApplicationHost<string[]>
    {
        protected CliApplicationHost(string applicationId, params Assembly[] assemblies) 
            : base(applicationId, assemblies)
        {
        }

        public async Task<int> Run(string[] arguments)
        {
            var services = new ServiceCollection();

            BuildContainer(services);

            try
            {
                Start(arguments);
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,"Error in {ApplicationId}",ApplicationId);
                return -1;
            }
            finally
            {
                Stop();
            }
        }
    }
}