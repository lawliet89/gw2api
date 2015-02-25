using System;
using gw2api.Model;
using gw2api.Object;
using ReactiveUI;
using Splat;

namespace PromotionViabilityWpf.ViewModel
{
    public class PriceCalculatorViewModel : ReactiveObject
    {
        public readonly IReactiveDerivedList<ItemBundledEntity> Items;
        private readonly ReactiveList<ItemBundledEntity> availableItems; 

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
        }
    }
}
