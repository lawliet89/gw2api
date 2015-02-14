using System;
using System.Collections.Generic;
using System.Linq;
using gw2api.Extension;
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
        public ItemBundledEntity Promoted { get; set; }
        public Yield QuantityYield { get; set; }
        public Dictionary<ItemBundledEntity, int> Ingredients { get; set; }

        public Coin CostOfAllIngredients
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
                return AverageProfitOfProduct - CostOfAllIngredients;
            }
        }

        public bool AverageProfitable
        {
            get
            {
                return AverageProfitOfPromotion > 0;
            }
        }

        public Coin ProfitOfProduct(int quantityYielded)
        {
            if (quantityYielded > QuantityYield.Upper || quantityYielded < QuantityYield.Lower)
                throw new ArgumentException("Quantity yield provided is outside the range of possible yield");

            return Coin.ProfitSellingAt(Promoted.MinSaleUnitPrice * quantityYielded);
        }

        /// <summary>
        /// Calculates the remaining cost of ingredients based on how many you have
        /// </summary>
        /// <returns></returns>
        public Coin CostOfIngredients(IDictionary<int, int> ingredients)
        {
            var totalCost = new Coin(0);
            foreach (var ingredient in Ingredients)
            {
                var quantity = ingredient.Value;
                var availableIngredient = ingredients.FirstOrDefault(i => i.Key == ingredient.Key.Identifier);
                if (!availableIngredient.IsDefault())
                {
                    quantity = Math.Max(0, quantity - ingredient.Value);
                }
                totalCost += quantity*ingredient.Key.MaxOfferUnitPrice;
            }
            return totalCost;
        }

        public Coin CostOfIngredients(IDictionary<ItemBundledEntity, int> ingredients)
        {
            return CostOfIngredients(ingredients.ToDictionary(pair => pair.Key.Identifier, pair => pair.Value));
        }

        public string Name
        {
            get { return Promoted.Object.Name; }
        }

        #region IBundleable Methods

        IEnumerable<IBundledEntity<int, Item>> IBundlelable<int, Item>.Entities
        {
            get { return Items; }
        }

        IEnumerable<IBundledEntity<int, AggregateListing>> IBundlelable<int, AggregateListing>.Entities
        {
            get { return Items; }
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

        public IEnumerable<ItemBundledEntity> Items
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

        public bool Populated
        {
            get
            {
                return ((IBundlelable<int, Item>)this).Entities.All(e => e.Object != null)
                       && ((IBundlelable<int, AggregateListing>)this).Entities.All(e => e.Object != null);
            }
        }
    }
}
