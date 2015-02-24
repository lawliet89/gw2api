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
        private static readonly IEnumerable<PromotionViewModel> AvailablePromotions = 
            Data.Promotions.FineMaterialsTier6Promotions.Select(x => new PromotionViewModel(x));

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ReactiveList<Task> activeTasks;
        private readonly ReactiveList<PromotionViewModel> promotions;

        public IReactiveDerivedList<PromotionViewModel> LoadedPromotions;

        private readonly ObservableAsPropertyHelper<Boolean> loading; 
        public Boolean IsLoading
        {
            get { return loading.Value; }
        }

        public ReactiveCommand<object> RefreshCommand { get; private set; }

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

            RefreshCommand = ReactiveCommand.Create();
            RefreshCommand.Subscribe(_ =>
            {
                ReloadPrices();
            });
        }

        #region Methods that must be invoked on the UI Thread/Context
        private async void Reload()
        {
            var allPromotions = promotions.Select(p => p.Promotion).ToList();
            await Task.WhenAll(LoadWithICons<int, Item>(allPromotions), 
                Load<int, AggregateListing>(allPromotions))
                .ConfigureAwait(false);
        }

        private async void ReloadPrices()
        {
            await Load<int, AggregateListing>(promotions.Select(p => p.Promotion))
                .ConfigureAwait(false);
        }

        private async void Load(PromotionViewModel vm)
        {
            var promotion = vm.Promotion.Yield().ToList();
            await Task.WhenAll(LoadWithICons<int, Item>(promotion), 
                Load<int, AggregateListing>(promotion))
                .ConfigureAwait(false);
        }

        private async Task<Unit> Load<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundlelable)
        {
            var command = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var list = bundlelable as IList<IBundlelable<TKey, TValue>> ?? bundlelable.ToList();
                var task = Bundler.Request(list);
                activeTasks.Add(task);
                var result = await task;
                Bundler.Set(list, result);
                
                activeTasks.Remove(task);
            });
            // TODO: Handle exceptions
            return await command.ExecuteAsyncTask()
                .ConfigureAwait(false);
        }

        private async Task<Unit> LoadWithICons<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundlelable)
            where TValue : IRenderable
        {
            var list = bundlelable as IList<IBundlelable<TKey, TValue>> ?? bundlelable.ToList();
            var iconBundlesables = list.Cast<IBundleableRenderable<TValue>>().ToList();
            var command = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var task = Bundler.LoadIcon(iconBundlesables);
                activeTasks.Add(task);
                var icons = await task;
                Bundler.SetIcons(iconBundlesables, icons);

                activeTasks.Remove(task);
            });

            await Load(list);
            // TODO: Handle exceptions
            return await command.ExecuteAsyncTask()
                .ConfigureAwait(false);
        }
        #endregion
    }
}
