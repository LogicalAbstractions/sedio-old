﻿using System.Collections.Generic;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IResponse : IMessage
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

        OperationCode OperationCode { get; set; }

        ResponseCode ResponseCode { get; set; }
    }
}