using System.Collections.Generic;
using System.Text;

namespace Sedio.Core.Collections
{
    /// <summary>Helpers for <see cref="KeyValueRef{TKey,TValue}"/>.</summary>
    public static class KeyValueRef   
    {
        /// <summary>Creates the key value pair.</summary>
        /// <typeparam name="TKey">Key type</typeparam> <typeparam name="TValue">Value type</typeparam>
        /// <param name="key">Key</param> <param name="value">Value</param> <returns>New pair.</returns>
        public static KeyValueRef<TKey, TValue> Of<TKey, TValue>(TKey key, TValue value) => new KeyValueRef<TKey, TValue>(key, value);

        /// <summary>Creates the new pair with new key and old value.</summary>
        /// <typeparam name="TKey">Key type</typeparam> <typeparam name="TValue">Value type</typeparam>
        /// <param name="source">Source value</param> <param name="key">New key</param> <returns>New pair</returns>
        public static KeyValueRef<TKey, TValue> WithKey<TKey, TValue>(this KeyValueRef<TKey, TValue> source, TKey key) => new KeyValueRef<TKey, TValue>(key, source.Value);

        /// <summary>Creates the new pair with old key and new value.</summary>
        /// <typeparam name="TKey">Key type</typeparam> <typeparam name="TValue">Value type</typeparam>
        /// <param name="source">Source value</param> <param name="value">New value.</param> <returns>New pair</returns>
        public static KeyValueRef<TKey, TValue> WithValue<TKey, TValue>(this KeyValueRef<TKey, TValue> source, TValue value) => new KeyValueRef<TKey, TValue>(source.Key, value);
    }

    /// <summary>Immutable Key-Value pair. It is reference type (could be check for null), 
    /// which is different from System value type <see cref="KeyValuePair{TKey,TValue}"/>.
    /// In addition provides <see cref="Equals"/> and <see cref="GetHashCode"/> implementations.</summary>
    /// <typeparam name="TKey">Type of Key.</typeparam><typeparam name="TValue">Type of Value.</typeparam>
    public class KeyValueRef<TKey, TValue>
    {
        /// <summary>Key.</summary>
        public readonly TKey Key;

        /// <summary>Value.</summary>
        public readonly TValue Value;

        /// <summary>Creates Key-Value object by providing key and value. Does Not check either one for null.</summary>
        /// <param name="key">key.</param><param name="value">value.</param>
        public KeyValueRef(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>Creates nice string view.</summary><returns>String representation.</returns>
        public override string ToString()
        {
            var s = new StringBuilder('{');
            if (Key != null)
                s.Append(Key);
            s.Append(',');
            if (Value != null)
                s.Append(Value);
            s.Append('}');
            return s.ToString();
        }

        /// <summary>Returns true if both key and value are equal to corresponding key-value of other object.</summary>
        /// <param name="obj">Object to check equality with.</param> <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            return obj is KeyValueRef<TKey, TValue> other
                   && (ReferenceEquals(other.Key, Key) || Equals(other.Key, Key))
                   && (ReferenceEquals(other.Value, Value) || Equals(other.Value, Value));
        }

        /// <summary>Combines key and value hash code. R# generated default implementation.</summary>
        /// <returns>Combined hash code for key-value.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((object)Key == null ? 0 : Key.GetHashCode() * 397)
                       ^ ((object)Value == null ? 0 : Value.GetHashCode());
            }
        }
    }
}