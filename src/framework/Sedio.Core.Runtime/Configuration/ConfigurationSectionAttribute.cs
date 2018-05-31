using System;
using System.Reflection;

namespace Sedio.Core.Runtime.Configuration
{
    public class ConfigurationSectionAttribute : System.Attribute
    {
        private readonly string name;
        
        public ConfigurationSectionAttribute(string name = null)
        {
            this.name = name;
        }

        internal static string GetSectionName(Type sectionType)
        {
            if (sectionType == null) throw new ArgumentNullException(nameof(sectionType));

            var attribute = sectionType.GetCustomAttribute<ConfigurationSectionAttribute>();

            return (attribute?.name ??
                    sectionType.Name.Replace("Configuration", string.Empty).Replace("Section", string.Empty))
                .ToLowerInvariant();
        }
    }
}