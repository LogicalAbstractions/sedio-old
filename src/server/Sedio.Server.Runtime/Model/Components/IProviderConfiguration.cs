using System;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sedio.Contracts.Components;

namespace Sedio.Server.Runtime.Model.Components
{
    public interface IProviderConfiguration
    {
        string ProviderId { get; set; }
        
        string ProviderParametersJson { get; set; }
    }

    public static class ProviderConfigurationExtensions
    {
        public static JObject GetProviderParameters(this IProviderConfiguration providerConfiguration)
        {
            if (providerConfiguration == null) throw new ArgumentNullException(nameof(providerConfiguration));

            if (!string.IsNullOrWhiteSpace(providerConfiguration.ProviderParametersJson))
            {
                return JObject.Parse(providerConfiguration.ProviderParametersJson);
            }

            return null;
        }

        public static T GetProviderParameters<T>(this IProviderConfiguration providerConfiguration,
            JsonSerializerSettings serializerSettings)
        {
            if (providerConfiguration == null) throw new ArgumentNullException(nameof(providerConfiguration));
            if (serializerSettings == null) throw new ArgumentNullException(nameof(serializerSettings));

            if (!string.IsNullOrWhiteSpace(providerConfiguration.ProviderParametersJson))
            {
                return JsonConvert.DeserializeObject<T>(providerConfiguration.ProviderParametersJson,
                    serializerSettings);
            }

            return default(T);
        }

        public static void CopyTo(this IProviderConfiguration input,IProviderConfigurationDto output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            
            output.ProviderId = input.ProviderId;
            output.ProviderParameters = input.GetProviderParameters();
        }

        public static void CopyTo(this IProviderConfigurationDto input, IProviderConfiguration output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.ProviderId = input.ProviderId;
            output.ProviderParametersJson = input.ProviderParameters?.ToString();
        }

        public static T ToOutput<T>(this IProviderConfiguration input)
            where T : class,IProviderConfigurationDto,new()
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = new T();
            input.CopyTo(result);
            return result;
        }

        public static T ToEntity<T>(this IProviderConfigurationDto input)
            where T : class,IProviderConfiguration,new()
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = new T();
            input.CopyTo(result);
            return result;
        }
    }
}