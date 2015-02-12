using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gw2api.Extension;
using GW2NET.Common;

namespace gw2api.Request
{
    public static class Bundler
    {
        public static Task<IDictionaryRange<TKey, TValue>> Request<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundles)
        {
            var bundlelables = bundles as IList<IBundlelable<TKey, TValue>> ?? bundles.ToList();
            var keys = bundlelables.SelectMany(b => b.Entities)
                .Select(e => e.Identifier);
            var service = bundlelables.First().Service;

            // It seems like starting an async operation is slow. Wrap it in another task
            return Task.Factory.StartNew(() => service.FindAllAsync(keys.ToList()))
                .Unwrap();
        }

        public static void Set<TKey, TValue>(IDictionaryRange<TKey, TValue> values,
            IEnumerable<IBundlelable<TKey, TValue>> bundles)
        {
            bundles.SelectMany(b => b.Entities).ForEach(e => e.Object = values[e.Identifier]);
        }

        public static Task BundleAndSet<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundles)
        {
            var bundlelables = bundles as IList<IBundlelable<TKey, TValue>> ?? bundles.ToList();
            return Request(bundlelables).Then(task =>
            {
                Set(task.Result, bundlelables);
            });
        }
    }
}
