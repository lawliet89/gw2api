using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using gw2api.Extension;
using gw2api.Object;
using gw2api.Request;
using GW2NET;
using GW2NET.Commerce;
using GW2NET.Common;
using GW2NET.Items;
using ReactiveUI;

namespace PromotionViabilityWpf.Model
{
    public class Promotion : ReactiveObject,
        IBundlelable<int, Item>, 
        IBundlelable<int, AggregateListing>,
        IBundleableRenderable<Item>
    {
        public ItemBundledEntity Promoted { get; private set; }
        public Yield QuantityYield { get; private set; }

        // To workaround the limitation of ReactiveUI
        public List<int> IngredientsQuantity { get; private set; }
        public ReactiveList<ItemBundledEntity> IngredientsEntities { get; private set; }

        public Promotion(ItemBundledEntity promoted, Dictionary<ItemBundledEntity, int> ingredients, Yield quantityYield)
        {
            Promoted = promoted;
            QuantityYield = quantityYield;
            IngredientsQuantity = new List<int>();
            IngredientsEntities = new ReactiveList<ItemBundledEntity>() { ChangeTrackingEnabled = true };

            Ingredients = ingredients;

            Items = new ReactiveList<ItemBundledEntity> { ChangeTrackingEnabled = true };

            Items.Add(Promoted);
            Items.AddRange(IngredientsEntities);

            Items.ItemChanged
                .Subscribe(_ => checkPopulated());
        }

        public Dictionary<ItemBundledEntity, int> Ingredients
        {
            get
            {
                return IngredientsEntities.Zip(IngredientsQuantity, (k, v) => new {k, v})
                    .ToDictionary(x => x.k, x => x.v);
            }
            private set
            {
                IngredientsEntities.Clear();
                IngredientsEntities.AddRange(value.Keys);
                IngredientsQuantity.Clear();
                IngredientsQuantity.AddRange(value.Values);
            }
        }

        public Coin CostOfAllIngredients
        {
            get
            {
                return IngredientsEntities.Zip(IngredientsQuantity, (i, q) => i.MaxOfferUnitPrice * q)
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
            foreach (var pair in IngredientsEntities.Zip(IngredientsQuantity, (i, q) => new { i, q }))
            {
                var ingredient = pair.i;
                var quantity = pair.q;
                var availableIngredient = ingredients.FirstOrDefault(i => i.Key == ingredient.Identifier);
                if (!availableIngredient.IsDefault())
                {
                    quantity = Math.Max(0, quantity - availableIngredient.Value);
                }
                totalCost += quantity*ingredient.MaxOfferUnitPrice;
            }
            return totalCost;
        }

        public Coin CostOfIngredients(IDictionary<ItemBundledEntity, int> ingredients)
        {
            return CostOfIngredients(ingredients.ToDictionary(pair => pair.Key.Identifier, pair => pair.Value));
        }

        private void checkPopulated()
        {
            Populated = Promoted.IconPng != null && Promoted.IconPng.Length > 0
                        && Items.All(i => i.Listings != null && i.Item != null);
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

        public ReactiveList<ItemBundledEntity> Items { get; private set; }

        public IEnumerable<IBundleableRenderableEntity<Item>> Renderables
        {
            get { return Promoted.Yield(); }
        }

        private bool populated; 
        public bool Populated
        {
            get { return populated; }
            set { this.RaiseAndSetIfChanged(ref populated, value); }
        }
    }
}
