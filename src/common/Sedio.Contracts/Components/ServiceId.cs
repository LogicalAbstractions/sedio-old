using System;

namespace Sedio.Contracts.Components
{
    public struct ServiceId : IEquatable<ServiceId>
    {
        private readonly string value;

        public ServiceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new System.ArgumentException("value must be present", nameof(value));
            }

            this.value = value;
        }

        public bool Equals(ServiceId other)
        {
            return string.Equals(value, other.value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ServiceId id && Equals(id);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(value);
        }

        public static bool operator ==(ServiceId left, ServiceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ServiceId left, ServiceId right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return value;
        }
    }
}