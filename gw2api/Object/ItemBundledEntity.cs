using GW2NET.Commerce;
using GW2NET.Items;

namespace gw2api.Object
{
    public class ItemBundledEntity : IBundledEntity<int, Item>, 
        IBundledEntity<int, AggregateListing>
    {

        private int id;
        private Item item;
        private AggregateListing listing;

        public ItemBundledEntity(int id)
        {
            this.id = id;
        }

        public Coin MaxOfferUnitPrice
        {
            get { return listing.BuyOffers.UnitPrice; }
        }

        public Coin MinSaleUnitPrice
        {
            get { return listing.SellOffers.UnitPrice; }
        }

        public Item Object
        {
            get { return item; }
            set { item = value; }
        }

        AggregateListing IBundledEntity<int, AggregateListing>.Object
        {
            get { return listing; }
            set { listing = value; }
        }

        public int Identifier
        {
            get { return id; }
            set { id = value; }
        }
    }
}