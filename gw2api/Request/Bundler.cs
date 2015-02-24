using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gw2api.Extension;
using GW2NET;
using GW2NET.Common;

namespace gw2api.Request
{
    public static class Bundler
    {
        public static string IconFormat = "png";
        public static IRenderService IconService = GW2.Rendering.RenderService;

        public static Task<IDictionaryRange<TKey, TValue>> Request<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundles)
        {
            var bundlelables = bundles as IList<IBundlelable<TKey, TValue>> ?? bundles.ToList();
            var keys = bundlelables.SelectMany(b => b.Entities)
                .Select(e => e.Identifier);
            var service = bundlelables.First().Service;

            // It seems like starting an async operation is slow. Wrap it in another task
            return Task.Run(() => service.FindAllAsync(keys.ToList()));
        }

        public static void Set<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundles, IDictionaryRange<TKey, TValue> values)
        {
            bundles.SelectMany(b => b.Entities).ForEach(e => e.Object = values[e.Identifier]);
        }

        public static Task BundleAndSet<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundles)
        {
            var bundlelables = bundles as IList<IBundlelable<TKey, TValue>> ?? bundles.ToList();
            return Request(bundlelables).Then(task =>
            {
                Set(bundlelables, task.Result);
            });
        }

        public static Task<Dictionary<T, byte[]>> LoadIcon<T>(IEnumerable<IBundleableRenderable<T>> bundleables)
            where T : IRenderable
        {
            return Task.Run(() =>
            {
                return bundleables.SelectMany(b => b.Renderables)
                    .Distinct()
                    .ToDictionary(r => r.Renderable, r => IconService.GetImage(r.Renderable, IconFormat));
            });
        }

        public static void SetIcons<T>(IEnumerable<IBundleableRenderable<T>> bundleables, Dictionary<T, byte[]> icons)
            where T : IRenderable
        {
            bundleables.SelectMany(b => b.Renderables)
                .ForEach(r => r.Icon = icons[r.Renderable]);
        }

        public static Task LoadAndSetIcons<T>(IEnumerable<IBundleableRenderable<T>> bundleables)
            where T : IRenderable
        {
            var list = bundleables as IList<IBundleableRenderable<T>> ?? bundleables.ToList();
            return LoadIcon(list)
                .Then(task =>
                {
                    SetIcons(list, task.Result);
                });
        }
    }
}
