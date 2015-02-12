using System;
using System.Collections.Generic;
using System.Linq;
using gw2api.Object;
using gw2api.Request;
using GW2NET;
using GW2NET.Commerce;
using GW2NET.Common;
using GW2NET.Items;

namespace gw2api.Model
{
    public class Promotion : IBundlelable<int, Item>, IBundlelable<int, AggregateListing>
    {
        public static int Tier5PromotionYield = 5;

        public ItemBundledEntity Promoted;
        public Yield QuantityYield;
        public Dictionary<ItemBundledEntity, int> Ingredients;

        public Coin CostOfIngridients
        {
            get
            {
                return Ingredients.Select(pair => pair.Key.MaxOfferUnitPrice * pair.Value)
                    .Aggregate((sum, itemCost) => sum + itemCost);
            }
        }

        public Coin AverageProfitOfProduct
        {
            get
            {
                return Coin.ProfitSellingAt(Promoted.MinSaleUnitPrice * QuantityYield.Average);
            }
        }

        public Coin AverageProfitOfPromotion
        {
            get
            {
                return AverageProfitOfProduct - CostOfIngridients;
            }
        }

        public bool Profitable
        {
            get
            {
                return AverageProfitOfPromotion > 0;
            }
        }

        public string Name
        {
            get { return Promoted.Object.Name; }
        }

        #region IBundleable Methods

        IEnumerable<IBundledEntity<int, Item>> IBundlelable<int, Item>.Entities
        {
            get
            {
                yield return Promoted;
                foreach (var item in Ingredients)
                {
                    yield return item.Key;
                }
            }
        }

        IEnumerable<IBundledEntity<int, AggregateListing>> IBundlelable<int, AggregateListing>.Entities
        {
            get
            {
                yield return Promoted;
                foreach (var item in Ingredients)
                {
                    yield return item.Key;
                }
            }
        }

        IRepository<int, AggregateListing> IBundlelable<int, AggregateListing>.Service
        {
            get { return GW2.V2.Commerce.Prices; }
        }

        public IRepository<int, Item> Service
        {
            get { return GW2.V2.Items.ForCurrentCulture(); }
        }

        #endregion
    }
}
