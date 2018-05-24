using System;

namespace Sedio.Core.Timing
{
    public sealed class SystemTimeProvider : ITimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}