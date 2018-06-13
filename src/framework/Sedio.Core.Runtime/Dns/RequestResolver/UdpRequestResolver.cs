using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Client;
using Sedio.Core.Runtime.Dns.Protocol;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.RequestResolver
{
    public class UdpRequestResolver : IRequestResolver
    {
        private int              timeout;
        private IRequestResolver fallback;
        private IPEndPoint       dns;

        public UdpRequestResolver(IPEndPoint dns, IRequestResolver fallback, int timeout = 5000)
        {
            this.dns = dns;
            this.fallback = fallback;
            this.timeout = timeout;
        }

        public UdpRequestResolver(IPEndPoint dns, int timeout = 5000)
        {
            this.dns = dns;
            this.fallback = new NullRequestResolver();
            this.timeout = timeout;
        }

        public async Task<IResponse> Resolve(IRequest request)
        {
            using (UdpClient udp = new UdpClient())
            {
                await udp
                    .SendAsync(request.ToArray(), request.Size, dns)
                    .WithCancellationTimeout(timeout);

                UdpReceiveResult result = await udp.ReceiveAsync().WithCancellationTimeout(timeout);
                if (!result.RemoteEndPoint.Equals(dns)) throw new IOException("Remote endpoint mismatch");
                byte[]          buffer   = result.Buffer;
                DefaultResponse response = DefaultResponse.FromArray(buffer);

                if (response.Truncated)
                {
                    return await fallback.Resolve(request);
                }

                return new ClientResponse(request, response, buffer);
            }
        }
    }
}