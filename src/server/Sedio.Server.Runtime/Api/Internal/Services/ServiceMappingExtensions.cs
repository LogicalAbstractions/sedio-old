using System;
using Newtonsoft.Json.Linq;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Api.Internal.Services
{
    public static class ServiceMappingExtensions
    {
        public static ServiceOutputDto ToOutput(this Service service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var healthParametersJson = service.HealthAggregation.ParametersJson;
            
            return new ServiceOutputDto(new ServiceId(service.ServiceId),
                new HealthAggregationConfigurationDto(service.HealthAggregation.ProviderId,healthParametersJson != null ? JObject.Parse(healthParametersJson) : new JObject() ),
                service.CacheTime,
                service.CreatedAt  );
        }
    }
}