using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Sedio.Core.Collections.Pooling
{
    public sealed class StaticAsyncObjectPool<T> : IAsyncObjectPool<T>,IDisposable
    {
        private readonly List<T> allItems = new List<T>();
        private readonly AsyncProducerConsumerQueue<T> items = new AsyncProducerConsumerQueue<T>();
        private readonly Action<T> resetter;

        public StaticAsyncObjectPool(IEnumerable<T> items,Action<T> resetter)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            this.resetter = resetter ?? throw new ArgumentNullException(nameof(resetter));
            
            foreach (var item in items)
            {
                this.allItems.Add(item);
                this.items.Enqueue(item);
            }
        }
        
        public Task<T> Aquire(CancellationToken cancellationToken)
        {
            return items.DequeueAsync(cancellationToken);
        }

        public void Release(T value)
        {
            resetter.Invoke(value);
            items.Enqueue(value);
        }

        public void Dispose()
        {
            foreach (var item in allItems.OfType<IDisposable>())
            {
                item.Dispose();
            }
        }
    }
}