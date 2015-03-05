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

        private readonly ObservableAsPropertyHelper<Coin> unitSellOfferPrice;

        public Coin UnitSellOfferPrice
        {
            get { return unitSellOfferPrice.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> unitBuyOfferPrice;
        public Coin UnitBuyOfferPrice
        {
            get { return unitBuyOfferPrice.Value; }
        }

        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { this.RaiseAndSetIfChanged(ref quantity, value); }
        }

        private readonly ObservableAsPropertyHelper<Coin> sellOfferPrice;

        public Coin SellOfferPrice
        {
            get { return sellOfferPrice.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> buyOfferPrice;
        public Coin BuyOfferPrice
        {
            get { return buyOfferPrice.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> sellOfferPriceTaxed;

        public Coin SellOfferPriceTaxed
        {
            get { return sellOfferPriceTaxed.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> buyOfferPriceTaxed;

        public Coin BuyOfferPriceTaxed
        {
            get { return buyOfferPriceTaxed.Value; }
        }

        public PriceCalculatorViewModel()
        {
            var service = Locator.Current.GetService<IObjectRepository<int, ItemBundledEntity>>();
            availableItems = new ReactiveList<ItemBundledEntity>(service.All) { ChangeTrackingEnabled = true };
            Quantity = 0;

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

            Items = availableItems.CreateDerivedCollection(i => i, i => i.Item != null, 
                (a, b) => String.Compare(a.Item.Name, b.Item.Name, StringComparison.Ordinal));

            this.WhenAnyValue(x => x.SelectedItem)
                .Where(i => i != null)
                .Select(i => i.IconPng.ToImage())
                .ToProperty(this, x => x.SelectedIcon, out selectedIcon);
            
            Observable.Merge(new []
            {
                this.WhenAnyValue(x => x.SelectedItem).Where(i => i != null).Select(i => i.MinSaleUnitPrice),
                this.WhenAnyValue(x => x.selectedItem.Listings).Select(l => (Coin) l.SellOffers.UnitPrice)
            }).ToProperty(this, x => x.UnitSellOfferPrice, out unitSellOfferPrice, 0);

            Observable.Merge(new[]
            {
                this.WhenAnyValue(x => x.SelectedItem).Where(i => i != null).Select(i => i.MaxOfferUnitPrice),
                this.WhenAnyValue(x => x.selectedItem.Listings).Select(l => (Coin) l.BuyOffers.UnitPrice)
            }).ToProperty(this, x => x.UnitBuyOfferPrice, out unitBuyOfferPrice, 0);

            Observable.Merge(new[]
            {
                this.WhenAnyValue(x => x.UnitSellOfferPrice).Select(_ => UnitSellOfferPrice*Quantity),
                this.WhenAnyValue(x => x.Quantity).Select(_ => UnitSellOfferPrice*Quantity),
            }).ToProperty(this, x => x.SellOfferPrice, out sellOfferPrice, 0);

            Observable.Merge(new[]
            {
                this.WhenAnyValue(x => x.UnitBuyOfferPrice).Select(_ => UnitBuyOfferPrice*Quantity),
                this.WhenAnyValue(x => x.Quantity).Select(_ => UnitBuyOfferPrice*Quantity),
            }).ToProperty(this, x => x.BuyOfferPrice, out buyOfferPrice, 0);

            this.WhenAnyValue(x => x.SellOfferPrice)
                .Select(Coin.ProfitSellingAt)
                .ToProperty(this, x => x.SellOfferPriceTaxed, out sellOfferPriceTaxed);

            this.WhenAnyValue(x => x.BuyOfferPrice)
                .Select(Coin.ProfitSellingAt)
                .ToProperty(this, x => x.BuyOfferPriceTaxed, out buyOfferPriceTaxed);
        }
    }
}
