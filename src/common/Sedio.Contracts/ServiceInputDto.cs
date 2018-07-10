using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceInputDto
    {
        public StatusAggregationConfigurationDto StatusAggregation { get; set; }
        
        public TimeSpan? CacheTime { get; set; }
    }
}