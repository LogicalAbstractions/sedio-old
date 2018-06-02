using System;
using System.Collections.Generic;

namespace Sedio.Core.Runtime.MemoryStore
{
    public interface ICollectionIndex
    {
        string PropertyName { get; }
        
        Type PropertyType { get; }

        ICollectionIndex Add(IList<object> documentBatch);
        
        
    }
}