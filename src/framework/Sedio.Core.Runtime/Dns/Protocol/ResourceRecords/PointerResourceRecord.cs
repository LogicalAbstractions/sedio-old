using System;
using System.Net;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol.ResourceRecords
{
    public class PointerResourceRecord : AbstractResourceRecord
    {
        public PointerResourceRecord(IResourceRecord record, byte[] message, int dataOffset)
            : base(record)
        {
            PointerDomainName = Domain.FromArray(message, dataOffset);
        }

        public PointerResourceRecord(IPAddress ip, Domain pointer, TimeSpan ttl = default(TimeSpan)) :
            base(new ResourceRecord(Domain.PointerName(ip), pointer.ToArray(), DnsRecordType.PTR, DnsRecordClass.IN, ttl))
        {
            PointerDomainName = pointer;
        }

        public Domain PointerDomainName { get; private set; }

        public override string ToString()
        {
            return Stringify().Add("PointerDomainName").ToString();
        }
    }
}