using System;
using System.Collections.Generic;
using ReactiveUI;

namespace gw2api.Object
{
    public interface IObjectRepository<TKey, TValue> : IReactiveNotifyCollectionChanged<TValue>, 
        IReactiveNotifyCollectionItemChanged<TValue>
        where TValue : IHasIdentifier<TKey>
    {
        TValue GetItem(TKey key);
        TValue GetOrAddItem(TKey key, Func<TKey, TValue> factory);
        void AddRange(IEnumerable<TValue> range);
        List<TValue> All { get; } 
    }
}
