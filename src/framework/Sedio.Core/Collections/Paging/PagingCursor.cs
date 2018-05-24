using System;

namespace Sedio.Core.Collections.Paging
{
    public struct PagingCursor : IEquatable<PagingCursor>
    {
        private readonly string value;

        public static readonly PagingCursor Start = new PagingCursor(null);

        public PagingCursor(string value)
        {
            this.value = value;
        }

        public bool IsStart => string.IsNullOrWhiteSpace(value);

        public override string ToString()
        {
            return value;
        }

        public bool Equals(PagingCursor other)
        {
            return string.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PagingCursor cursor && Equals(cursor);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public static bool operator ==(PagingCursor left, PagingCursor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PagingCursor left, PagingCursor right)
        {
            return !left.Equals(right);
        }
    }
}