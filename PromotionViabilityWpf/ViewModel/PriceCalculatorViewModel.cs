using System;
using gw2api.Model;
using gw2api.Object;
using ReactiveUI;
using Splat;

namespace PromotionViabilityWpf.ViewModel
{
    public class PriceCalculatorViewModel : ReactiveObject
    {
        public readonly IReactiveDerivedList<ItemBundledEntity> LoadedAvailableItems;
        private readonly ReactiveList<ItemBundledEntity> availableItems;

        private ItemBundledEntity selectedItem;
        public ItemBundledEntity SelectedItem
        {
            get { return selectedItem; }
            set { this.RaiseAndSetIfChanged(ref selectedItem, value); }
        }

        public readonly ReactiveList<PriceCalculatorItemViewModel> Items; 

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

            LoadedAvailableItems = availableItems.CreateDerivedCollection(i => i, i => i.Item != null, 
                (a, b) => String.Compare(a.Item.Name, b.Item.Name, StringComparison.Ordinal));

            Items = new ReactiveList<PriceCalculatorItemViewModel>();

            this.WhenAnyValue(x => x.SelectedItem)
                .Subscribe(item =>
                {
                    if (item != null)
                    {
                        Items.Add(new PriceCalculatorItemViewModel(item));
                    }
                });
        }
    }
}
