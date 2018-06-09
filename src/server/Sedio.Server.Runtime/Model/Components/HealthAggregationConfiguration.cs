namespace Sedio.Server.Runtime.Model.Components
{
    public class HealthAggregationConfiguration : IProviderConfiguration
    {
        public string ProviderId { get; set; }
        
        public string ProviderParametersJson { get; set; }
    }
}