using System;

namespace Sedio.Core.Runtime.Dns.Protocol.ResourceRecords
{
    public interface IResourceRecord : IMessageEntry
    {
        TimeSpan TimeToLive { get; }

        int DataLength { get; }

        byte[] Data { get; }
    }
}