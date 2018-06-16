using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Protocol;

namespace Sedio.Core.Runtime.Dns.RequestResolver
{
    public interface IDnsRequestResolver
    {
        Task<IDnsResponse> Resolve(IDnsRequest request);
    }
}