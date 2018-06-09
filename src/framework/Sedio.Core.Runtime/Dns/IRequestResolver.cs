using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Protocol;

namespace Sedio.Core.Runtime.Dns
{
    public interface IRequestResolver
    {
        Task<IResponse> Resolve(IRequest request);
    }
}