using System;
using System.Net;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol.ResourceRecords
{
    public class IPAddressResourceRecord : AbstractResourceRecord
    {
        private static IResourceRecord Create(Domain domain, IPAddress ip, TimeSpan ttl)
        {
            byte[]     data = ip.GetAddressBytes();
            DnsRecordType type = data.Length == 4 ? DnsRecordType.A : DnsRecordType.AAAA;

            return new ResourceRecord(domain, data, type, DnsRecordClass.IN, ttl);
        }

        public IPAddressResourceRecord(IResourceRecord record) : base(record)
        {
            IPAddress = new IPAddress(Data);
        }

        public IPAddressResourceRecord(Domain domain, IPAddress ip, TimeSpan ttl = default(TimeSpan)) :
            base(Create(domain, ip, ttl))
        {
            IPAddress = ip;
        }

        public IPAddress IPAddress { get; private set; }

        public override string ToString()
        {
            return Stringify().Add("IPAddress").ToString();
        }
    }
}