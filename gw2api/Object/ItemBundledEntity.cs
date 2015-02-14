using GW2NET.Commerce;
using GW2NET.Items;

namespace gw2api.Object
{
    public class ItemBundledEntity : IBundledEntity<int, Item>, 
        IBundledEntity<int, AggregateListing>
    {
        public Item Item { get; private set; }
        public AggregateListing Listings { get; private set; }
        public byte[] IconPng { get; private set; }

        public ItemBundledEntity(int id)
        {
            this.Identifier = id;
        }

        public Coin MaxOfferUnitPrice
        {
            get { return Listings.BuyOffers.UnitPrice; }
        }

        public Coin MinSaleUnitPrice
        {
            get { return Listings.SellOffers.UnitPrice; }
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
    }
}