using System.Threading;
using System.Threading.Tasks;

namespace Sedio.Core.Collections.Pooling
{
    public interface IAsyncObjectPool<T>
    {
        Task<T> Aquire(CancellationToken cancellationToken);

        void Release(T value);
    }
}