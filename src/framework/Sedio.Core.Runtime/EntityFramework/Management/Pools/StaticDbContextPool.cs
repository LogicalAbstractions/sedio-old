using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;

namespace Sedio.Core.Runtime.EntityFramework.Management.Pools
{
    public sealed class StaticDbContextPool<T> : IDbContextPool<T>
        where T : DbContext
    {
        internal sealed class StaticDbContextHandle : IDbContextHandle<T>
        {
            private readonly StaticDbContextPool<T> pool;
            
            internal StaticDbContextHandle(StaticDbContextPool<T> pool,T context)
            {
                this.pool = pool;
                Context = context;
            }

            public void Dispose()
            {
                pool.Release(Context);
            }

            public T Context { get; }
        }
        
        private readonly AsyncProducerConsumerQueue<T> items;
        private readonly List<T> allItems;

        public StaticDbContextPool(IDbContextFactory<T> contextFactory,int size)
        {
            if (contextFactory == null) throw new ArgumentNullException(nameof(contextFactory));

            this.allItems = Enumerable.Range(0, size).Select(i => contextFactory.CreateInstance()).ToList();
            this.items = new AsyncProducerConsumerQueue<T>(allItems);
        }

        public async Task<IDbContextHandle<T>> Aquire(CancellationToken cancellationToken)
        {
            var context = await items.DequeueAsync(cancellationToken).ConfigureAwait(false);
            
            return new StaticDbContextHandle(this,context);
        }

        public void Dispose()
        {
            foreach (var item in allItems.OfType<IDisposable>())
            {
                item.Dispose();
            }
        }

        private void Release(T context)
        {
            items.Enqueue(context);
        }
    }
}