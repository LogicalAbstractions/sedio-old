namespace Sedio.Server.Runtime.Model.Components
{
    public class InstanceRoutingConfiguration : IProviderConfiguration
    {
        public string ProviderId { get; set; }
        
        public string ProviderParametersJson { get; set; }
    }
}