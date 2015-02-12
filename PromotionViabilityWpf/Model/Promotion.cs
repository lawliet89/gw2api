using System;
using System.Collections.Generic;
using System.Linq;
using gw2api.Object;
using gw2api.Request;
using GW2NET;
using GW2NET.Commerce;
using GW2NET.Common;
using GW2NET.Items;

namespace PromotionViabilityWpf.Model
{
    class Promotion : IBundlelable<int, Item>, IBundlelable<int, AggregateListing>
    {
        public static int Tier5PromotionYield = 5;

        public ItemWrapper Promoted;
        public int QuantityYield;
        public Dictionary<ItemWrapper, int> Ingredients;

        public Coin CostOfIngridients
        {
            get
            {
                return Ingredients.Select(pair => pair.Key.MaxOfferUnitPrice * pair.Value)
                    .Aggregate((sum, itemCost) => sum + itemCost);
            }
        }

        public Coin ProfitOfProduct
        {
            get
            {
                return Coin.ProfitSellingAt(Promoted.MinSaleUnitPrice * QuantityYield);
            }
        }

        public Coin ProfitOfPromotion
        {
            get
            {
                return ProfitOfProduct - CostOfIngridients;
            }
        }

        public bool Profitable
        {
            get
            {
                return ProfitOfPromotion > 0;
            }
        }

        public string Name
        {
            get { return Promoted.Object.Name; }
        }

        #region IBundleable Methods

        public IEnumerable<int> GetKeys(Type valueType)
        {
            yield return Promoted.Identifier;
            foreach (var pair in Ingredients)
            {
                yield return pair.Key.Identifier;
            }
        }

        public void SetValue(int key, Item value)
        {
            if (key == Promoted.Identifier)
            {
                Promoted.Object = value;
            }
            
            var ingredient = Ingredients.SingleOrDefault(pair => pair.Key.Identifier == key);
            if (ingredient.Key.Identifier == key)
            {
                ingredient.Key.Object = value;
            }
                
        }

        public void SetValue(int key, AggregateListing value)
        {
            if (key == Promoted.Identifier)
            {
                Promoted.MaxOfferUnitPrice = value.BuyOffers.UnitPrice;
                Promoted.MinSaleUnitPrice = value.SellOffers.UnitPrice;
            }
            var ingredient = Ingredients.Single(pair => pair.Key.Identifier == key);
            if (ingredient.Key.Identifier == key)
            {
                ingredient.Key.MaxOfferUnitPrice = value.BuyOffers.UnitPrice;
                ingredient.Key.MinSaleUnitPrice = value.SellOffers.UnitPrice;   
            }
        }

        IRepository<int, AggregateListing> IBundlelable<int, AggregateListing>.GetService()
        {
            return GW2.V2.Commerce.Prices;
        }

        public IRepository<int, Item> GetService()
        {
            return GW2.V2.Items.ForCurrentCulture();
        }

        #endregion
    }
}
