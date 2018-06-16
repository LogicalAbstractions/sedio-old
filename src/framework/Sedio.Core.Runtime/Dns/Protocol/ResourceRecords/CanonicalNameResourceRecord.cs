using System;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol.ResourceRecords
{
    public class CanonicalNameResourceRecord : AbstractResourceRecord
    {
        public CanonicalNameResourceRecord(IResourceRecord record, byte[] message, int dataOffset)
            : base(record)
        {
            CanonicalDomainName = Domain.FromArray(message, dataOffset);
        }

        public CanonicalNameResourceRecord(Domain domain, Domain cname, TimeSpan ttl = default(TimeSpan)) :
            base(new ResourceRecord(domain, cname.ToArray(), DnsRecordType.CNAME, DnsRecordClass.IN, ttl))
        {
            CanonicalDomainName = cname;
        }

        public Domain CanonicalDomainName { get; private set; }

        public override string ToString()
        {
            return Stringify().Add("CanonicalDomainName").ToString();
        }
    }
}