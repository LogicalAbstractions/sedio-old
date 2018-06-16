using System.Collections.Generic;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IDnsResponse : IDnsMessage
    {
        int Id { get; set; }

        IList<IResourceRecord> AnswerRecords { get; }

        IList<IResourceRecord> AuthorityRecords { get; }

        IList<IResourceRecord> AdditionalRecords { get; }

        bool RecursionAvailable { get; set; }

        bool AuthenticData { get; set; }

        bool CheckingDisabled { get; set; }

        bool AuthorativeServer { get; set; }

        bool Truncated { get; set; }

        DnsOperationCode OperationCode { get; set; }

        DnsResponseCode ResponseCode { get; set; }
    }
}