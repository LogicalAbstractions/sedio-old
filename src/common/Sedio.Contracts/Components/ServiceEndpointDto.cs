using Newtonsoft.Json;

namespace Sedio.Contracts.Components
{
    public sealed class ServiceEndpointDto
    {
        public string Protocol { get; set; }

        public int? Port { get; set; }
    }
}