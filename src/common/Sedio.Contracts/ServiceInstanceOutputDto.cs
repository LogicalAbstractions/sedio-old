using System;
using System.Collections.Generic;
using System.Net;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceInstanceOutputDto
    {
        public ServiceInstanceOutputDto(IPAddress address,SemanticVersion version, DateTimeOffset createdAt)
        {
            Address = address;
            Version = version;
            CreatedAt = createdAt;
        }

        public IPAddress Address { get; }

        public SemanticVersion Version { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}