using System;

namespace Sedio.Core.Runtime.MemoryStore
{
    public readonly struct DocumentEntry
    {
        public string Id { get; }
        
        public object Document { get; }
        
        public DateTimeOffset CreatedAt { get; }
        
        public DateTimeOffset UpdatedAt { get; }
    }
}