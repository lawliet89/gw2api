using System;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using gw2api.Model;
using gw2api.Object;
using PromotionViabilityWpf.Extensions;
using ReactiveUI;
using Splat;

namespace PromotionViabilityWpf.ViewModel
{
    public class PriceCalculatorViewModel : ReactiveObject
    {
        public readonly IReactiveDerivedList<ItemBundledEntity> Items;
        private readonly ReactiveList<ItemBundledEntity> availableItems;

        private ItemBundledEntity selectedItem;
        public ItemBundledEntity SelectedItem
        {
            get { return selectedItem; }
            set { this.RaiseAndSetIfChanged(ref selectedItem, value); }
        }

        private readonly ObservableAsPropertyHelper<BitmapImage> selectedIcon;

        public BitmapImage SelectedIcon
        {
            get { return selectedIcon.Value; }
        }

        public PriceCalculatorViewModel()
        {
            var service = Locator.Current.GetService<IObjectRepository<int, ItemBundledEntity>>();
            availableItems = new ReactiveList<ItemBundledEntity>(service.All) { ChangeTrackingEnabled = true };

            service.ItemsAdded
                .Subscribe(i =>
                {
                    availableItems.Add(i);
                });
            service.ItemsRemoved
                .Subscribe(i =>
                {
                    availableItems.Remove(i);
                });
            service.ShouldReset
                .Subscribe(_ =>
                {
                    availableItems.Clear();
                    availableItems.AddRange(service.All);
                });

            Items = availableItems.CreateDerivedCollection(i => i, i => i.Item != null);

            this.WhenAnyValue(x => x.SelectedItem)
                .Where(i => i != null)
                .Select(i => i.IconPng.ToImage())
                .ToProperty(this, x => x.SelectedIcon, out selectedIcon);
        }
    }
}
