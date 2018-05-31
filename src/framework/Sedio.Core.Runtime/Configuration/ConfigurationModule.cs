using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Sedio.Core.Runtime.Application.Assemblies;

namespace Sedio.Core.Runtime.Configuration
{
    public sealed class ConfigurationModule : Autofac.Module
    {
        private readonly IAssemblyProvider assemblyProvider;

        public ConfigurationModule(IAssemblyProvider assemblyProvider)
        {
            this.assemblyProvider = assemblyProvider ?? throw new ArgumentNullException(nameof(assemblyProvider));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(assemblyProvider.Assemblies).Where(t =>
                    t.IsClass && !t.IsAbstract && t.GetCustomAttribute<ConfigurationSectionAttribute>() != null)
                .OnActivating(args =>
                {
                    var configuration = args.Context.Resolve<IConfiguration>();
                    configuration.TryBindConfigurationSection(args.Instance);

                }).SingleInstance();   
        }
    }
}