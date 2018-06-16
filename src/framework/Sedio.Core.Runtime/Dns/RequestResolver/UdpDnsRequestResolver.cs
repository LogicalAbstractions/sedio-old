using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Client;
using Sedio.Core.Runtime.Dns.Protocol;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.RequestResolver
{
    public class UdpDnsRequestResolver : IDnsRequestResolver
    {
        private int              timeout;
        private IDnsRequestResolver fallback;
        private IPEndPoint       dns;

        public UdpDnsRequestResolver(IPEndPoint dns, IDnsRequestResolver fallback, int timeout = 5000)
        {
            this.dns = dns;
            this.fallback = fallback;
            this.timeout = timeout;
        }

        public UdpDnsRequestResolver(IPEndPoint dns, int timeout = 5000)
        {
            this.dns = dns;
            this.fallback = new NullDnsRequestResolver();
            this.timeout = timeout;
        }

        public async Task<IDnsResponse> Resolve(IDnsRequest request)
        {
            using (UdpClient udp = new UdpClient())
            {
                await udp
                    .SendAsync(request.ToArray(), request.Size, dns)
                    .WithCancellationTimeout(timeout);

                UdpReceiveResult result = await udp.ReceiveAsync().WithCancellationTimeout(timeout);
                if (!result.RemoteEndPoint.Equals(dns)) throw new IOException("Remote endpoint mismatch");
                byte[]          buffer   = result.Buffer;
                DefaultDnsResponse response = DefaultDnsResponse.FromArray(buffer);

                if (response.Truncated)
                {
                    return await fallback.Resolve(request);
                }

                return new ClientDnsResponse(request, response, buffer);
            }
        }
    }
}