using System;
using System.Collections.Generic;
using System.Net;
using NuGet.Versioning;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceInstanceOutputDto
    {
        public IPAddress Address { get; set; }

        public SemanticVersion Version { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}