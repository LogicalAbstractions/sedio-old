using System;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceStatusOutputDto
    {
        public StatusType Status { get; set; }

        public string Message { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}