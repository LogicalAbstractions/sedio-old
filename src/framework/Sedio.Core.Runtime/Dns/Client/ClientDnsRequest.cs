using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Protocol;
using Sedio.Core.Runtime.Dns.Protocol.ResourceRecords;
using Sedio.Core.Runtime.Dns.RequestResolver;

namespace Sedio.Core.Runtime.Dns.Client
{
    public class ClientDnsRequest : IDnsRequest
    {
        private const int DEFAULT_PORT = 53;

        private IDnsRequestResolver resolver;
        private IDnsRequest         request;

        public ClientDnsRequest(IPEndPoint dns, IDnsRequest request = null) :
            this(new UdpDnsRequestResolver(dns), request)
        {
        }

        public ClientDnsRequest(IPAddress ip, int port = DEFAULT_PORT, IDnsRequest request = null) :
            this(new IPEndPoint(ip, port), request)
        {
        }

        public ClientDnsRequest(string ip, int port = DEFAULT_PORT, IDnsRequest request = null) :
            this(IPAddress.Parse(ip), port, request)
        {
        }

        public ClientDnsRequest(IDnsRequestResolver resolver, IDnsRequest request = null)
        {
            this.resolver = resolver;
            this.request = request == null ? new DefaultDnsRequest() : new DefaultDnsRequest(request);
        }

        public int Id
        {
            get { return request.Id; }
            set { request.Id = value; }
        }

        public IList<IResourceRecord> AdditionalRecords
        {
            get { return new ReadOnlyCollection<IResourceRecord>(request.AdditionalRecords); }
        }

        public DnsOperationCode OperationCode
        {
            get { return request.OperationCode; }
            set { request.OperationCode = value; }
        }

        public bool RecursionDesired
        {
            get { return request.RecursionDesired; }
            set { request.RecursionDesired = value; }
        }

        public IList<DnsQuestion> Questions
        {
            get { return request.Questions; }
        }

        public int Size
        {
            get { return request.Size; }
        }

        public byte[] ToArray()
        {
            return request.ToArray();
        }

        public override string ToString()
        {
            return request.ToString();
        }

        /// <summary>
        /// Resolves this request into a response using the provided DNS information. The given
        /// request strategy is used to retrieve the response.
        /// </summary>
        /// <exception cref="DnsResponseException">Throw if a malformed response is received from the server</exception>
        /// <exception cref="IOException">Thrown if a IO error occurs</exception>
        /// <exception cref="SocketException">Thrown if the reading or writing to the socket fails</exception>
        /// <exception cref="OperationCanceledException">Thrown if reading or writing to the socket timeouts</exception>
        /// <returns>The response received from server</returns>
        public async Task<IDnsResponse> Resolve()
        {
            try
            {
                IDnsResponse response = await resolver.Resolve(this);

                if (response.Id != this.Id)
                {
                    throw new DnsResponseException(response, "Mismatching request/response IDs");
                }

                if (response.ResponseCode != DnsResponseCode.NoError)
                {
                    throw new DnsResponseException(response);
                }

                return response;
            }
            catch (ArgumentException e)
            {
                throw new DnsResponseException("Invalid response", e);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new DnsResponseException("Invalid response", e);
            }
        }
    }
}