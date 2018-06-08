using System;
using System.Collections.Generic;
using System.Linq;

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

    public static class PagingResultExtensions
    {
        public static PagingResult<T> ToPagedResult<T>(this IEnumerable<T> input, PagingParameters pagingParameters)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pagingParameters == null) throw new ArgumentNullException(nameof(pagingParameters));

            var offset = pagingParameters.Cursor.IsStart ? 0 : long.Parse(pagingParameters.Cursor.ToString());

            var totalCount = input.LongCount();

            var resultItems = input.Skip((int) offset).Take(pagingParameters.Limit).ToList();

            var continuationCursor = PagingCursor.FromOffset(offset + resultItems.Count);
            
            return new PagingResult<T>(resultItems,totalCount,continuationCursor);
        }

        public static PagingResult<TOut> Map<TIn, TOut>(this PagingResult<TIn> input, Func<TIn, TOut> mappingFunction)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (mappingFunction == null) throw new ArgumentNullException(nameof(mappingFunction));
            
            return new PagingResult<TOut>(input.Items.Select(mappingFunction).ToList(),input.TotalItemCount,input.ContinuationCursor);
        }
    }
}