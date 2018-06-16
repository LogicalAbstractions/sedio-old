using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sedio.Core.Runtime.Dns.Protocol;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;

namespace Sedio.Core.Runtime.Dns.Client
{
    public class ClientDnsResponse : IDnsResponse
    {
        private IDnsResponse response;
        private byte[]    message;

        public static ClientDnsResponse FromArray(IDnsRequest request, byte[] message)
        {
            DefaultDnsResponse response = DefaultDnsResponse.FromArray(message);
            return new ClientDnsResponse(request, response, message);
        }

        internal ClientDnsResponse(IDnsRequest request, IDnsResponse response, byte[] message)
        {
            Request = request;

            this.message = message;
            this.response = response;
        }

        internal ClientDnsResponse(IDnsRequest request, IDnsResponse response)
        {
            Request = request;

            this.message = response.ToArray();
            this.response = response;
        }

        public IDnsRequest Request { get; private set; }

        public int Id
        {
            get { return response.Id; }
            set { }
        }

        public IList<IResourceRecord> AnswerRecords
        {
            get { return response.AnswerRecords; }
        }

        public IList<IResourceRecord> AuthorityRecords
        {
            get { return new ReadOnlyCollection<IResourceRecord>(response.AuthorityRecords); }
        }

        public IList<IResourceRecord> AdditionalRecords
        {
            get { return new ReadOnlyCollection<IResourceRecord>(response.AdditionalRecords); }
        }

        public bool RecursionAvailable
        {
            get { return response.RecursionAvailable; }
            set { }
        }

        public bool AuthenticData
        {
            get { return response.AuthenticData; }
            set { }
        }

        public bool CheckingDisabled
        {
            get { return response.CheckingDisabled; }
            set { }
        }

        public bool AuthorativeServer
        {
            get { return response.AuthorativeServer; }
            set { }
        }

        public bool Truncated
        {
            get { return response.Truncated; }
            set { }
        }

        public DnsOperationCode OperationCode
        {
            get { return response.OperationCode; }
            set { }
        }

        public DnsResponseCode ResponseCode
        {
            get { return response.ResponseCode; }
            set { }
        }

        public IList<DnsQuestion> Questions
        {
            get { return new ReadOnlyCollection<DnsQuestion>(response.Questions); }
        }

        public int Size
        {
            get { return message.Length; }
        }

        public byte[] ToArray()
        {
            return message;
        }

        public override string ToString()
        {
            return response.ToString();
        }
    }
}