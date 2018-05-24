using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sedio.Contracts
{
    public sealed class InstanceInputDto
    {
        [JsonConstructor]
        public InstanceInputDto(IReadOnlyDictionary<string, object> tags)
        {
            Tags = tags;
        }

        public IReadOnlyDictionary<string,object> Tags { get; }
    }
}