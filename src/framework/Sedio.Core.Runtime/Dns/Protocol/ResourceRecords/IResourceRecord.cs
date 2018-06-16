using System;

namespace Sedio.Core.Runtime.Dns.Protocol.ResourceRecords
{
    public interface IResourceRecord : IDnsMessageEntry
    {
        TimeSpan TimeToLive { get; }

        int DataLength { get; }

        byte[] Data { get; }
    }
}