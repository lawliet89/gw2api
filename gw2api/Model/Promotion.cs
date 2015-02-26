using System;
using System.Collections.Generic;
using System.Linq;
using gw2api.Extension;
using gw2api.Object;
using gw2api.Request;
using GW2NET.Commerce;
using GW2NET.Items;
using Newtonsoft.Json;
using ReactiveUI;

namespace gw2api.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Promotion : ReactiveObject,
        IBundlelable<int, Item>, 
        IBundlelable<int, AggregateListing>,
        IBundleableRenderable<Item>,
        IHasIdentifier<int>
    {
        public static readonly IObjectRepository<int, Promotion> Repository = new ObjectRepository<int, Promotion>();

        [JsonProperty]
        public ItemBundledEntity Promoted { get; private set; }
        [JsonProperty]
        public Yield QuantityYield { get; private set; }

        // To workaround the limitation of ReactiveUI
        public List<int> IngredientsQuantity { get; private set; }
        public ReactiveList<ItemBundledEntity> IngredientsEntities { get; private set; }

        public static Promotion Get(int id)
        {
            return Repository.GetItem(id);
        }

        public static Promotion Get(ItemBundledEntity promoted)
        {
            return Get(promoted.Identifier);
        }

        public static Promotion GetOrCreate(ItemBundledEntity promoted, Dictionary<ItemBundledEntity, int> ingredients,
            Yield quantityYield)
        {
            return Repository.GetOrAddItem(promoted.Identifier, _ => new Promotion(promoted, ingredients, quantityYield));
        }

        private Promotion(ItemBundledEntity promoted, Dictionary<ItemBundledEntity, int> ingredients, Yield quantityYield)
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

        [JsonProperty]
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
            Populated = Items.All(i => i.IconPng != null && i.IconPng.Length > 0 
                && i.Listings != null && i.Item != null);
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
        #endregion

        public ReactiveList<ItemBundledEntity> Items { get; private set; }

        public IEnumerable<IBundleableRenderableEntity<Item>> Renderables
        {
            get { return Items; }
        }

        private bool populated; 
        public bool Populated
        {
            get { return populated; }
            set { this.RaiseAndSetIfChanged(ref populated, value); }
        }

        // For now, we are going to simplify things and make the Promoted Item be the Promotion model ID
        // We might want to support multiple recipes for the same Promoted item in the future
        public int Identifier
        {
            get { return Promoted.Identifier; }
        }
    }
}
