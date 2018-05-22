using System;
using System.Collections.Generic;
using System.Net;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceInstanceOutputDto
    {
        public ServiceInstanceOutputDto(IPAddress address,SemanticVersion version, IReadOnlyDictionary<string, object> tags, DateTimeOffset createdAt)
        {
            Address = address;
            Version = version;
            Tags = tags;
            CreatedAt = createdAt;
        }

        public IPAddress Address { get; }

        public SemanticVersion Version { get; }

        public IReadOnlyDictionary<string,object> Tags { get; }

        public DateTimeOffset CreatedAt { get; }
    }
}