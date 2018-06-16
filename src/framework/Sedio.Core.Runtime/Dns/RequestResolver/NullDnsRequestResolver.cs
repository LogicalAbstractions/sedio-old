using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Client;
using Sedio.Core.Runtime.Dns.Protocol;

namespace Sedio.Core.Runtime.Dns.RequestResolver
{
    public class NullDnsRequestResolver : IDnsRequestResolver
    {
        public Task<IDnsResponse> Resolve(IDnsRequest request)
        {
            throw new DnsResponseException("Request failed");
        }
    }
}