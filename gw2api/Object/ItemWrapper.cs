using GW2NET.Items;

namespace gw2api.Object
{
    public class ItemWrapper : Wrapper<int, Item>
    {
        public ItemWrapper(int id) : base(id)
        {
            
        }

        public Currency MaxOfferUnitPrice { get; set; }
        public Currency MinSaleUnitPrice { get; set; }
    }
}