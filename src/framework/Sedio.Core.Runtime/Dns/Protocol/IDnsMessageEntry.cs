using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IDnsMessageEntry
    {
        Domain Name { get; }

        DnsRecordType Type { get; }

        DnsRecordClass Class { get; }

        int Size { get; }

        byte[] ToArray();
    }
}