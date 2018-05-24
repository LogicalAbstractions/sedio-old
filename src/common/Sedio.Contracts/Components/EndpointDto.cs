using Newtonsoft.Json;

namespace Sedio.Contracts.Components
{
    public sealed class EndpointDto
    {
        [JsonConstructor]
        public EndpointDto(string protocol, int port)
        {
            if (string.IsNullOrWhiteSpace(protocol))
            {
                throw new System.ArgumentException("protocol must be specified", nameof(protocol));
            }

            Protocol = protocol;
            Port = port;
        }

        public string Protocol { get; }

        public int Port { get; }
    }
}