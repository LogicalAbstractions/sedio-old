using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sedio.Core.Runtime.Configuration
{
    public static class ConfigurationExtensions
    {
        public static bool TryBindConfigurationSection(this IConfiguration configuration, object configurationSection)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            if (configurationSection.GetType().GetCustomAttribute<ConfigurationSectionAttribute>() == null)
            {
                throw new InvalidOperationException(
                    $"Cannot bind type not marked with [ConfigurationSectionAttribute] as a configuration section: ${configurationSection.GetType().Name}");
            }

            var sectionName = ConfigurationSectionAttribute.GetSectionName(configurationSection.GetType());

            var sectionData = configuration.GetSection(sectionName);

            if (sectionData != null)
            {
                sectionData.Bind(configurationSection);
                return true;
            }

            return false;
        }
    }
}