using System;
using System.Collections.Generic;
using Sedio.Contracts.Components;

namespace Sedio.Contracts
{
    public sealed class ServiceOutputDto
    {
        public string Id { get; set; }

        public StatusAggregationConfigurationDto StatusAggregation { get; set; }

        public TimeSpan? CacheTime { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}