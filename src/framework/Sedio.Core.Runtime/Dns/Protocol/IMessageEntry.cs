namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IMessageEntry
    {
        Domain Name { get; }

        RecordType Type { get; }

        RecordClass Class { get; }

        int Size { get; }

        byte[] ToArray();
    }
}