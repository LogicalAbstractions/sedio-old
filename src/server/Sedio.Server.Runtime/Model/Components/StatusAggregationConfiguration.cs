﻿namespace Sedio.Server.Runtime.Model.Components
{
    public class StatusAggregationConfiguration : IProviderConfiguration
    {
        public string ProviderId { get; set; }
        
        public string ProviderParametersJson { get; set; }
    }
}