using System;
using System.Collections.Generic;
using GW2NET.Common;

namespace gw2api.Request
{
    /// <summary>
    /// An interface that allows bundling up of Service requests
    /// <typeparam name="TKey">The identifier type related to the service</typeparam>
    /// <typeparam name="TValue">Type of objects returned by service</typeparam>
    /// </summary>
    public interface IBundlelable<TKey, TValue>
    {
        IEnumerable<TKey> GetKeys(Type valueType);
        void SetValue(TKey key, TValue value);
        IRepository<TKey, TValue> GetService();
    }
}
