using System;

namespace Sedio.Core.Collections.Paging
{
    public sealed class PagingParameters
    {
        public const int DefaultLimit = 8;
        public const int MaxLimit = 64;

        public PagingParameters(PagingCursor cursor, int limit = DefaultLimit)
        {
            Cursor = cursor;
            Limit = limit;
        }

        public PagingCursor Cursor { get; }

        public int          Limit { get; }

        public PagingParameters Coerce()
        {
            var finalLimit = Limit < 1 ? 1 : Limit;
            finalLimit = finalLimit > MaxLimit ? MaxLimit : finalLimit;

            return new PagingParameters(Cursor,finalLimit);
        }
    }
}