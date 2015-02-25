using System.Collections.Generic;
using gw2api.Object;

namespace gw2api.Request
{
    /// <summary>
    /// An interface that allows bundling up of Service requests
    /// <typeparam name="TKey">The identifier type related to the service</typeparam>
    /// <typeparam name="TValue">Type of objects returned by service</typeparam>
    /// </summary>
    public interface IBundlelable<out TKey, TValue>
    {
        IEnumerable<IBundledEntity<TKey, TValue>> Entities { get; }
    }
}
