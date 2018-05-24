using System;
using System.Collections.Generic;

namespace Sedio.Core.Collections.Paging
{
    public sealed class PagingResult<T> 
    {
        public PagingResult(IReadOnlyList<T> items, long totalItemCount, PagingCursor continuationCursor)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalItemCount = totalItemCount;
            ContinuationCursor = continuationCursor;
        }

        public IReadOnlyList<T> Items { get; }

        public long TotalItemCount { get; }

        public PagingCursor ContinuationCursor { get; }
    }
}