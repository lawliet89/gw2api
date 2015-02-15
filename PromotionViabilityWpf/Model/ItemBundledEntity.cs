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
        public Item Item { get; private set; }
        public AggregateListing Listings { get; private set; }
        public byte[] IconPng { get; private set; }

        public ItemBundledEntity(int id)
        {
            Identifier = id;
        }

        public Coin MaxOfferUnitPrice
        {
            get
            {
                if (Listings != null) return Listings.BuyOffers.UnitPrice;
                return null;
            }
        }

        public Coin MinSaleUnitPrice
        {
            get
            {
                if (Listings != null) return Listings.SellOffers.UnitPrice;
                return null;
            }
        }

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
    }
}