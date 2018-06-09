using Newtonsoft.Json;

namespace Sedio.Contracts.Components
{
    public sealed class EndpointDto
    {
        public string Protocol { get; set; }

        public int Port { get; set; }
    }
}