using Newtonsoft.Json;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceStatusInputDto
    {
        public StatusType Status { get; set; }

        public string Message { get; set; }
    }
}