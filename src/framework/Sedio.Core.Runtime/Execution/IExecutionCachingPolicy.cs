using System;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionCachingPolicy
    {
        TimeSpan? StaticCacheTime { get; }
    }
}