using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Protocol;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;
using Sedio.Core.Runtime.Dns.Protocol.Utils;
using Sedio.Core.Runtime.Dns.RequestResolver;

namespace Sedio.Core.Runtime.Dns.Client
{
    public class DnsClient
    {
        private const           int    DEFAULT_PORT = 53;
        private static readonly Random RANDOM       = new Random();

        private IDnsRequestResolver resolver;

        public DnsClient(IPEndPoint dns) :
            this(new UdpDnsRequestResolver(dns, new TcpDnsRequestResolver(dns)))
        {
        }

        public DnsClient(IPAddress ip, int port = DEFAULT_PORT) :
            this(new IPEndPoint(ip, port))
        {
        }

        public DnsClient(string ip, int port = DEFAULT_PORT) :
            this(IPAddress.Parse(ip), port)
        {
        }

        public DnsClient(IDnsRequestResolver resolver)
        {
            this.resolver = resolver;
        }

        public ClientDnsRequest FromArray(byte[] message)
        {
            DefaultDnsRequest request = DefaultDnsRequest.FromArray(message);
            return new ClientDnsRequest(resolver, request);
        }

        public ClientDnsRequest Create(IDnsRequest request = null)
        {
            return new ClientDnsRequest(resolver, request);
        }

        public async Task<IList<IPAddress>> Lookup(string domain, DnsRecordType type = DnsRecordType.A)
        {
            if (type != DnsRecordType.A && type != DnsRecordType.AAAA)
            {
                throw new ArgumentException("Invalid record type " + type);
            }

            IDnsResponse response = await Resolve(domain, type);
            IList<IPAddress> ips = response.AnswerRecords
                .Where(r => r.Type == type)
                .Cast<IPAddressResourceRecord>()
                .Select(r => r.IPAddress)
                .ToList();

            if (ips.Count == 0)
            {
                throw new DnsResponseException(response, "No matching records");
            }

            return ips;
        }

        public Task<string> Reverse(string ip)
        {
            return Reverse(IPAddress.Parse(ip));
        }

        public async Task<string> Reverse(IPAddress ip)
        {
            IDnsResponse       response = await Resolve(Domain.PointerName(ip), DnsRecordType.PTR);
            IResourceRecord ptr      = response.AnswerRecords.FirstOrDefault(r => r.Type == DnsRecordType.PTR);

            if (ptr == null)
            {
                throw new DnsResponseException(response, "No matching records");
            }

            return ((PointerResourceRecord) ptr).PointerDomainName.ToString();
        }

        public Task<IDnsResponse> Resolve(string domain, DnsRecordType type)
        {
            return Resolve(new Domain(domain), type);
        }

        public Task<IDnsResponse> Resolve(Domain domain, DnsRecordType type)
        {
            ClientDnsRequest request  = Create();
            DnsQuestion      question = new DnsQuestion(domain, type);

            request.Questions.Add(question);
            request.OperationCode = DnsOperationCode.Query;
            request.RecursionDesired = true;

            return request.Resolve();
        }
    }
}