using System.Reactive.Linq;
using gw2api.Object;
using GW2NET.Commerce;
using GW2NET.Items;
using ReactiveUI;

namespace PromotionViabilityWpf.Model
{
    public class ItemBundledEntity : ReactiveObject,
        IBundledEntity<int, Item>, 
        IBundledEntity<int, AggregateListing>,
        IBundleableRenderableEntity<Item>
    {
        public ItemBundledEntity(int id)
        {
            Identifier = id;

            this.WhenAnyValue(x => x.Listings)
                .Where(l => l != null)
                .Select(l => (Coin)l.BuyOffers.UnitPrice)
                .ToProperty(this, x => x.MaxOfferUnitPrice, out maxOfferUnitPrice, 0);
            this.WhenAnyValue(x => x.Listings)
                .Where(l => l != null)
                .Select(l => (Coin)l.SellOffers.UnitPrice)
                .ToProperty(this, x => x.MinSaleUnitPrice, out minSaleUnitPrice, 0);
        }

        private Item item;

        public Item Item
        {
            get { return item; }
            private set { this.RaiseAndSetIfChanged(ref item, value); }
        }

        private AggregateListing listings;

        public AggregateListing Listings
        {
            get { return listings; }
            private set { this.RaiseAndSetIfChanged(ref listings, value); }
        }

        private byte[] iconPng;

        public byte[] IconPng
        {
            get { return iconPng; }
            private set { this.RaiseAndSetIfChanged(ref iconPng, value); }
        }

        private readonly ObservableAsPropertyHelper<Coin> maxOfferUnitPrice; 
        public Coin MaxOfferUnitPrice
        {
            get { return maxOfferUnitPrice.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> minSaleUnitPrice; 
        public Coin MinSaleUnitPrice
        {
            get { return minSaleUnitPrice.Value; }
        }

        #region IBundledEntity Overrides
        public Item Object
        {
            get { return Item; }
            set { Item = value; }
        }

        AggregateListing IBundledEntity<int, AggregateListing>.Object
        {
            get { return Listings; }
            set { Listings = value; }
        }

        public int Identifier { get; private set; }

        public Item Renderable
        {
            get { return Item; }
        }

        public byte[] Icon
        {
            set { IconPng = value; }
        }
        #endregion
    }
}