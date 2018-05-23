using Newtonsoft.Json;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class StatusInputDto
    {
        [JsonConstructor]
        public StatusInputDto(HealthStatusType status, string message)
        {
            Status = status;
            Message = message;
        }

        public HealthStatusType Status { get; }

        public string Message { get; }
    }
}