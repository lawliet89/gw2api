using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using ReactiveUI;

namespace gw2api.Object
{
    public class ObjectRepository<TKey, TValue> : IObjectRepository<TKey, TValue>
        where TValue : IHasIdentifier<TKey>
    {

        // Protects the cacheList field when adding or retrieving objects
        private readonly object cacheListMutex = new object(); 
        private readonly ReactiveList<TValue> cacheList;

        public ObjectRepository()
        {
            cacheList = new ReactiveList<TValue>();
        }

        public IDisposable SuppressChangeNotifications()
        {
            return cacheList.SuppressChangeNotifications();
        }

        public IObservable<TValue> ItemsAdded
        {
            get { return cacheList.ItemsAdded; }
        }

        public IObservable<TValue> BeforeItemsAdded
        {
            get { return cacheList.BeforeItemsAdded; }
        }

        public IObservable<TValue> ItemsRemoved
        {
            get { return cacheList.ItemsRemoved; }
        }

        public IObservable<TValue> BeforeItemsRemoved
        {
            get { return cacheList.BeforeItemsRemoved; }
        }

        public IObservable<IMoveInfo<TValue>> BeforeItemsMoved
        {
            get { return cacheList.BeforeItemsMoved; }
        }

        public IObservable<IMoveInfo<TValue>> ItemsMoved
        {
            get { return cacheList.ItemsMoved; }
        }

        public IObservable<NotifyCollectionChangedEventArgs> Changing
        {
            get { return cacheList.Changing; }
        }

        public IObservable<NotifyCollectionChangedEventArgs> Changed
        {
            get { return cacheList.Changed; }
        }

        public IObservable<int> CountChanging
        {
            get { return cacheList.CountChanging; }
        }

        public IObservable<int> CountChanged
        {
            get { return cacheList.CountChanged; }
        }

        public IObservable<bool> IsEmptyChanged
        {
            get { return cacheList.IsEmptyChanged; }
        }

        public IObservable<Unit> ShouldReset
        {
            get { return cacheList.ShouldReset; }
        }

        public IObservable<IReactivePropertyChangedEventArgs<TValue>> ItemChanging
        {
            get { return cacheList.ItemChanging; }
        }

        public IObservable<IReactivePropertyChangedEventArgs<TValue>> ItemChanged
        {
            get { return cacheList.ItemChanged; }
        }

        public bool ChangeTrackingEnabled
        {
            get { return cacheList.ChangeTrackingEnabled; }
            set { cacheList.ChangeTrackingEnabled = value; }
        }

        public TValue GetItem(TKey key)
        {
            lock (cacheListMutex)
            {
                return cacheList.SingleOrDefault(value => value.Identifier.Equals(key));
            }
        }

        public TValue GetOrAddItem(TKey key, Func<TKey, TValue> factory)
        {
            lock (cacheListMutex)
            {
                var item = GetItem(key);
                if (item != null) return item;
                var addedValue = factory(key);
                cacheList.Add(addedValue);
                return addedValue;
            }
        }


        public void AddRange(IEnumerable<TValue> range)
        {
            lock (cacheListMutex)
            {
                var rangeList = range as IList<TValue> ?? range.ToList();
                var keys = rangeList.Select(v => v.Identifier);
                if (cacheList.Select(v => v.Identifier).Intersect(keys).Any())
                {
                    throw new InvalidOperationException("Duplicate keys to add to repository");
                }
                cacheList.AddRange(rangeList);
            }
        }

        public List<TValue> All
        {
            get
            {
                lock (cacheListMutex)
                {
                    return cacheList.ToList();
                }
            }
        }
    }
}
