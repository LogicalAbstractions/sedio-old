using System.Collections.Generic;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IRequest : IMessage
    {
        int Id { get; set; }

        IList<IResourceRecord> AdditionalRecords { get; }

        OperationCode OperationCode { get; set; }

        bool RecursionDesired { get; set; }
    }
}