using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Collections.Paging;

namespace Sedio.Core.Runtime.Collections
{
    public static class PagingExtensions
    {
        public static async Task<PagingResult<T>> ToPagedResult<T>(this IQueryable<T> input, PagingParameters pagingParameters,CancellationToken cancellationToken)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pagingParameters == null) throw new ArgumentNullException(nameof(pagingParameters));

            var offset = pagingParameters.Cursor.IsStart ? 0 : long.Parse(pagingParameters.Cursor.ToString());

            var totalCount = await input.LongCountAsync(cancellationToken).ConfigureAwait(false);

            var resultItems = await input.Skip((int) offset).Take(pagingParameters.Limit).ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var continuationCursor = PagingCursor.FromOffset(offset + resultItems.Count);
            
            return new PagingResult<T>(resultItems,totalCount,continuationCursor);
        }
    }
}