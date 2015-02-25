using ReactiveUI;

namespace gw2api.Object
{
    public interface IObjectRepository<in TKey, TValue> : IReactiveNotifyCollectionChanged<TValue>, 
        IReactiveNotifyCollectionItemChanged<TValue>
        where TValue : IHasIdentifier<TKey>
    {
        TValue GetItem(TKey key);
        TValue GetOrAddItem(TKey key, TValue addedValue);
    }
}
