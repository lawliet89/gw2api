using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using gw2api.Extension;
using gw2api.Request;
using GW2NET.Commerce;
using GW2NET.Common;
using GW2NET.Items;
using ReactiveUI;

namespace PromotionViabilityWpf.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        private static readonly List<PromotionViewModel> AvailablePromotions;

        static MainWindowViewModel()
        {
            AvailablePromotions =
                new List<PromotionViewModel>(
                    Data.Promotions.FineMaterialsTier6Promotions.Select(x => new PromotionViewModel(x)));
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ReactiveList<Task> activeTasks;
        private readonly ReactiveList<PromotionViewModel> promotions;

        public IReactiveDerivedList<PromotionViewModel> LoadedPromotions;

        private readonly ObservableAsPropertyHelper<Boolean> loading; 
        public Boolean IsLoading
        {
            get { return loading.Value; }
        }

        internal MainWindowViewModel()
        {
            activeTasks = new ReactiveList<Task>();

            this.WhenAnyValue(x => x.activeTasks.Count)
                .Select(x => x != 0)
                .ToProperty(this, x => x.IsLoading, out loading);

            // Create list and add initial items
            promotions = new ReactiveList<PromotionViewModel>() { ChangeTrackingEnabled = true };
            promotions.AddRange(AvailablePromotions);
            Reload();

            // Set up behaviours
            promotions.ItemsAdded
                .Subscribe(Load);
            promotions.ShouldReset.Subscribe(_ => Reload());

            LoadedPromotions = promotions.CreateDerivedCollection(x => x, vm => vm.Populated);
        }

        public async void Reload()
        {
            var allPromotions = promotions.Select(p => p.Promotion).ToList();
            await LoadWithICons<int, Item>(allPromotions);
            await Load<int, AggregateListing>(allPromotions);
        }

        public async void ReloadPrices()
        {
            await Load<int, AggregateListing>(promotions.Select(p => p.Promotion));
        }

        private async void Load(PromotionViewModel vm)
        {
            var promotion = vm.Promotion.Yield().ToList();
            await LoadWithICons<int, Item>(promotion);
            await Load<int, AggregateListing>(promotion);
        }

        private Task<Unit> Load<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundlelable)
        {
            var command = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var task = Bundler.BundleAndSet(bundlelable);
                activeTasks.Add(task);
                await task;

                activeTasks.Remove(task);
                CheckPopulated();
            });
            // TODO: Handle exceptions
            return command.ExecuteAsyncTask();
        }

        private async Task<Task<Unit>> LoadWithICons<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundlelable)
            where TValue : IRenderable
        {
            var list = bundlelable as IList<IBundlelable<TKey, TValue>> ?? bundlelable.ToList();
            await Load(list);
            var iconBundlesables = list.Cast<IBundleableRenderable<TValue>>();

            var command = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var task = Bundler.LoadAndSetIcons(iconBundlesables);
                activeTasks.Add(task);
                await task;

                activeTasks.Remove(task);
                CheckPopulated();
            });
            // TODO: Handle exceptions
            return command.ExecuteAsyncTask();
        }

        private void CheckPopulated()
        {
            promotions.ForEach(p => p.CheckPopulated());
        }
    }
}
