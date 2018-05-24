using System;
using Sedio.Core.Collections.Paging;

namespace Sedio.Core.Converters
{
    public sealed class PagingCursorJsonConverter : StringJsonConverter<PagingCursor>
    {
        public override bool CanRead { get; } = false;

        protected override bool OnFromString(string value, out PagingCursor result)
        {
            throw new NotImplementedException();
        }
    }
}