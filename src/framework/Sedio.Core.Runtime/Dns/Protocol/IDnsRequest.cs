using System.Collections.Generic;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IDnsRequest : IDnsMessage
    {
        int Id { get; set; }

        IList<IResourceRecord> AdditionalRecords { get; }

        DnsOperationCode OperationCode { get; set; }

        bool RecursionDesired { get; set; }
    }
}