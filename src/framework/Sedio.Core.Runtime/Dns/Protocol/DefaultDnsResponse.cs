using System;
using System.Collections.Generic;
using System.Linq;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public class DefaultDnsResponse : IDnsResponse
    {
        private static readonly Random RANDOM = new Random();

        private DnsMessageHeader                 header;
        private IList<DnsQuestion>        questions;
        private IList<IResourceRecord> answers;
        private IList<IResourceRecord> authority;
        private IList<IResourceRecord> additional;

        public static DefaultDnsResponse FromRequest(IDnsRequest request)
        {
            DefaultDnsResponse response = new DefaultDnsResponse();

            response.Id = request.Id;

            foreach (DnsQuestion question in request.Questions)
            {
                response.Questions.Add(question);
            }

            return response;
        }

        public static DefaultDnsResponse FromArray(byte[] message)
        {
            DnsMessageHeader header = DnsMessageHeader.FromArray(message);
            int    offset = header.Size;

            if (!header.Response || header.QuestionCount == 0)
            {
                throw new ArgumentException("Invalid response message");
            }

            if (header.Truncated)
            {
                return new DefaultDnsResponse(header,
                    DnsQuestion.GetAllFromArray(message, offset, header.QuestionCount),
                    new List<IResourceRecord>(),
                    new List<IResourceRecord>(),
                    new List<IResourceRecord>());
            }

            return new DefaultDnsResponse(header,
                DnsQuestion.GetAllFromArray(message, offset, header.QuestionCount, out offset),
                ResourceRecordFactory.GetAllFromArray(message, offset, header.AnswerRecordCount, out offset),
                ResourceRecordFactory.GetAllFromArray(message, offset, header.AuthorityRecordCount, out offset),
                ResourceRecordFactory.GetAllFromArray(message, offset, header.AdditionalRecordCount, out offset));
        }

        public DefaultDnsResponse(DnsMessageHeader header, IList<DnsQuestion> questions, IList<IResourceRecord> answers,
                        IList<IResourceRecord> authority, IList<IResourceRecord> additional)
        {
            this.header = header;
            this.questions = questions;
            this.answers = answers;
            this.authority = authority;
            this.additional = additional;
        }

        public DefaultDnsResponse()
        {
            this.header = new DnsMessageHeader();
            this.questions = new List<DnsQuestion>();
            this.answers = new List<IResourceRecord>();
            this.authority = new List<IResourceRecord>();
            this.additional = new List<IResourceRecord>();

            this.header.Response = true;
            this.header.Id = RANDOM.Next(UInt16.MaxValue);
        }

        public DefaultDnsResponse(IDnsResponse response)
        {
            this.header = new DnsMessageHeader();
            this.questions = new List<DnsQuestion>(response.Questions);
            this.answers = new List<IResourceRecord>(response.AnswerRecords);
            this.authority = new List<IResourceRecord>(response.AuthorityRecords);
            this.additional = new List<IResourceRecord>(response.AdditionalRecords);

            this.header.Response = true;

            Id = response.Id;
            RecursionAvailable = response.RecursionAvailable;
            AuthorativeServer = response.AuthorativeServer;
            OperationCode = response.OperationCode;
            ResponseCode = response.ResponseCode;
        }

        public IList<DnsQuestion> Questions
        {
            get { return questions; }
        }

        public IList<IResourceRecord> AnswerRecords
        {
            get { return answers; }
        }

        public IList<IResourceRecord> AuthorityRecords
        {
            get { return authority; }
        }

        public IList<IResourceRecord> AdditionalRecords
        {
            get { return additional; }
        }

        public int Id
        {
            get { return header.Id; }
            set { header.Id = value; }
        }

        public bool RecursionAvailable
        {
            get { return header.RecursionAvailable; }
            set { header.RecursionAvailable = value; }
        }

        public bool AuthenticData
        {
            get { return header.AuthenticData; }
            set { header.AuthenticData = value; }
        }

        public bool CheckingDisabled
        {
            get { return header.CheckingDisabled; }
            set { header.CheckingDisabled = value; }
        }

        public bool AuthorativeServer
        {
            get { return header.AuthorativeServer; }
            set { header.AuthorativeServer = value; }
        }

        public bool Truncated
        {
            get { return header.Truncated; }
            set { header.Truncated = value; }
        }

        public DnsOperationCode OperationCode
        {
            get { return header.OperationCode; }
            set { header.OperationCode = value; }
        }

        public DnsResponseCode ResponseCode
        {
            get { return header.ResponseCode; }
            set { header.ResponseCode = value; }
        }

        public int Size
        {
            get
            {
                return header.Size +
                       questions.Sum(q => q.Size) +
                       answers.Sum(a => a.Size) +
                       authority.Sum(a => a.Size) +
                       additional.Sum(a => a.Size);
            }
        }

        public byte[] ToArray()
        {
            UpdateHeader();
            ByteStream result = new ByteStream(Size);

            result
                .Append(header.ToArray())
                .Append(questions.Select(q => q.ToArray()))
                .Append(answers.Select(a => a.ToArray()))
                .Append(authority.Select(a => a.ToArray()))
                .Append(additional.Select(a => a.ToArray()));

            return result.ToArray();
        }

        public override string ToString()
        {
            UpdateHeader();

            return ObjectStringifier.New(this)
                .Add("Header", header)
                .Add("Questions", "AnswerRecords", "AuthorityRecords", "AdditionalRecords")
                .ToString();
        }

        private void UpdateHeader()
        {
            header.QuestionCount = questions.Count;
            header.AnswerRecordCount = answers.Count;
            header.AuthorityRecordCount = authority.Count;
            header.AdditionalRecordCount = additional.Count;
        }
    }
}