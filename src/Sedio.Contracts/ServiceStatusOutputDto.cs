using System;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceStatusOutputDto
    {
        public ServiceStatusOutputDto(HealthStatusType status, string message, DateTimeOffset createdAt)
        {
            Status = status;
            Message = message;
            CreatedAt = createdAt;
        }

        public HealthStatusType Status { get; }

        public string Message { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}