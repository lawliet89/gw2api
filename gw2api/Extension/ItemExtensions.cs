using gw2api.Object;
using GW2NET;
using GW2NET.Items;

namespace gw2api.Extension
{
    public static class ItemExtensions
    {
        public static Coin MaxOfferUnitPrice(this Item item)
        {
            var commerceService = GW2.V2.Commerce.Prices;
            return commerceService.Find(item.ItemId)
                .BuyOffers.UnitPrice;
        }
    }
}
