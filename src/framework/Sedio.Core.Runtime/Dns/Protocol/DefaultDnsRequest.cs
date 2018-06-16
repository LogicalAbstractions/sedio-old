using System;
using System.Collections.Generic;
using System.Linq;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public class DefaultDnsRequest : IDnsRequest
    {
        private static readonly Random RANDOM = new Random();

        private IList<DnsQuestion>        questions;
        private DnsMessageHeader                 header;
        private IList<IResourceRecord> additional;

        public static DefaultDnsRequest FromArray(byte[] message)
        {
            DnsMessageHeader header = DnsMessageHeader.FromArray(message);
            int    offset = header.Size;

            if (header.Response || header.QuestionCount == 0 ||
                header.AnswerRecordCount + header.AuthorityRecordCount > 0 ||
                header.ResponseCode != DnsResponseCode.NoError)
            {
                throw new ArgumentException("Invalid request message");
            }

            return new DefaultDnsRequest(header,
                DnsQuestion.GetAllFromArray(message, offset, header.QuestionCount, out offset),
                ResourceRecordFactory.GetAllFromArray(message, offset, header.AdditionalRecordCount, out offset));
        }

        public DefaultDnsRequest(DnsMessageHeader header, IList<DnsQuestion> questions, IList<IResourceRecord> additional)
        {
            this.header = header;
            this.questions = questions;
            this.additional = additional;
        }

        public DefaultDnsRequest()
        {
            this.questions = new List<DnsQuestion>();
            this.header = new DnsMessageHeader();
            this.additional = new List<IResourceRecord>();

            this.header.OperationCode = DnsOperationCode.Query;
            this.header.Response = false;
            this.header.Id = RANDOM.Next(UInt16.MaxValue);
        }

        public DefaultDnsRequest(IDnsRequest request)
        {
            this.header = new DnsMessageHeader();
            this.questions = new List<DnsQuestion>(request.Questions);
            this.additional = new List<IResourceRecord>(request.AdditionalRecords);

            this.header.Response = false;

            Id = request.Id;
            OperationCode = request.OperationCode;
            RecursionDesired = request.RecursionDesired;
        }

        public IList<DnsQuestion> Questions
        {
            get { return questions; }
        }

        public IList<IResourceRecord> AdditionalRecords
        {
            get { return additional; }
        }

        public int Size
        {
            get
            {
                return header.Size +
                       questions.Sum(q => q.Size) +
                       additional.Sum(a => a.Size);
            }
        }

        public int Id
        {
            get { return header.Id; }
            set { header.Id = value; }
        }

        public DnsOperationCode OperationCode
        {
            get { return header.OperationCode; }
            set { header.OperationCode = value; }
        }

        public bool RecursionDesired
        {
            get { return header.RecursionDesired; }
            set { header.RecursionDesired = value; }
        }

        public byte[] ToArray()
        {
            UpdateHeader();
            ByteStream result = new ByteStream(Size);

            result
                .Append(header.ToArray())
                .Append(questions.Select(q => q.ToArray()))
                .Append(additional.Select(a => a.ToArray()));

            return result.ToArray();
        }

        public override string ToString()
        {
            UpdateHeader();

            return ObjectStringifier.New(this)
                .Add("Header", header)
                .Add("Questions", "AdditionalRecords")
                .ToString();
        }

        private void UpdateHeader()
        {
            header.QuestionCount = questions.Count;
            header.AdditionalRecordCount = additional.Count;
        }
    }
}