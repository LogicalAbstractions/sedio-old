using System;
using Sedio.Core.Collections;
using Sedio.Core.Collections.Immutable;

namespace Sedio.Server.Runtime.Persistence.Memory
{
    public sealed class DocumentIndex<TKey,TValue> : IDocumentIndex
    {
        private readonly ImHashMap<TKey, TValue> entries;
        private readonly Func<TValue, TKey> keyAccessor;
        private readonly string keyName;

        public DocumentIndex(string keyName,Func<TValue,TKey> keyAccessor) 
            : this(ImHashMap<TKey, TValue>.Empty,keyName,keyAccessor)
        {

        }

        public string KeyName => keyName;

        private DocumentIndex(ImHashMap<TKey, TValue> entries,string keyName,Func<TValue,TKey> keyAccessor)
        {
            this.entries = entries;
            this.keyName = keyName;
            this.keyAccessor = keyAccessor;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            return entries.TryFind(key, out value);
        }

        public DocumentIndex<TKey, TValue> Add(TValue value)
        {
            return new DocumentIndex<TKey, TValue>(entries.AddOrUpdate(keyAccessor.Invoke(value),value),keyName,keyAccessor);
        }

        public DocumentIndex<TKey, TValue> Update(TValue value)
        {
            return new DocumentIndex<TKey, TValue>(entries.AddOrUpdate(keyAccessor.Invoke(value),value),keyName,keyAccessor);
        }

        public DocumentIndex<TKey, TValue> Remove(TValue value)
        {
            return Remove(keyAccessor.Invoke(value));
        }

        public DocumentIndex<TKey, TValue> Remove(TKey key)
        {
            return new DocumentIndex<TKey, TValue>(entries.Remove(key),keyName,keyAccessor);
        }
    }
}