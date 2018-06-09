using System;
using Newtonsoft.Json.Linq;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Components;

namespace Sedio.Server.Runtime.Api.Internal.Services
{
    public static class ServiceMappingExtensions
    {
        public static ServiceOutputDto ToOutput(this Service service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            return new ServiceOutputDto()
            {
                Id                = new ServiceId(service.ServiceId),
                CacheTime         = service.CacheTime,
                CreatedAt         = service.CreatedAt,
                HealthAggregation = service.HealthAggregation.ToOutput<HealthAggregationConfigurationDto>()
            };
        }
    }
}