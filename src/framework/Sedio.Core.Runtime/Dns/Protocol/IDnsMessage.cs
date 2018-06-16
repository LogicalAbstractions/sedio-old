using System.Collections.Generic;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IDnsMessage
    {
        IList<DnsQuestion> Questions { get; }

        int Size { get; }

        byte[] ToArray();
    }
}