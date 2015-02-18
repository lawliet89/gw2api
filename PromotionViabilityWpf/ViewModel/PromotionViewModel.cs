using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using gw2api.Object;
using PromotionViabilityWpf.Extensions;
using PromotionViabilityWpf.Model;
using ReactiveUI;
using Splat;

namespace PromotionViabilityWpf.ViewModel
{
    public class PromotionViewModel : ReactiveObject
    {
        public Promotion Promotion { get; private set; }

        public PromotionViewModel(Promotion promotion)
        {
            Promotion = promotion;
            QuantityYield = promotion.QuantityYield.Average;

            this.WhenAnyValue(x => x.Promotion.Populated)
                .ToProperty(this, x => x.Populated, out populated);

            ingredientsQuantities = new ReactiveList<int>(promotion.IngredientsEntities.Select(_=>  0));

            var ingredientsObservables = new[]
            {
                ingredientsQuantities.ItemChanged.Select(_ => promotion.CostOfIngredients(Ingredients)),
                ingredientsQuantities.ShouldReset.Select(_ => promotion.CostOfIngredients(Ingredients)),
                Promotion.IngredientsEntities.ItemChanged.Select(_ => promotion.CostOfIngredients(Ingredients)),
                Promotion.IngredientsEntities.ShouldReset.Select(_ => promotion.CostOfIngredients(Ingredients)),
            };
            var mergedIngredientsObservables = Observable.Merge(ingredientsObservables);

            mergedIngredientsObservables
                .Subscribe(_ =>
                {
                    this.Log().Info("Ingredients change detected");
                });

            mergedIngredientsObservables
                .ToProperty(this, x => x.Cost, out cost);

            this.WhenAnyValue(x => x.Promotion.Promoted.MinSaleUnitPrice)
                .ToProperty(this, x => x.PromotedMinUnitSalePrice, out promotedMinUnitSalePrice);

            Observable.Merge(new[]
            {
                this.WhenAnyValue(x => x.QuantityYield),
                this.WhenAnyValue(x => x.PromotedMinUnitSalePrice).Select(_ => QuantityYield)
            })
                .Select(promotion.ProfitOfProduct)
                .ToProperty(this, x => x.ProfitOfProduct, out profitOfProduct);
        }

        public string Name
        {
            get { return Promotion.Name; }
        }

        private int quantityYield;

        public int QuantityYield
        {
            get { return quantityYield; }
            set { this.RaiseAndSetIfChanged(ref quantityYield, value); }
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ObservableAsPropertyHelper<Coin> profitOfProduct;

        public Coin ProfitOfProduct
        {
            get { return profitOfProduct.Value; }
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ObservableAsPropertyHelper<Coin> cost;

        public Coin Cost
        {
            get { return cost.Value; }
        }

        private ObservableAsPropertyHelper<bool> populated;

        public bool Populated
        {
            get { return populated.Value; }
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ObservableAsPropertyHelper<Coin> promotedMinUnitSalePrice;

        public Coin PromotedMinUnitSalePrice
        {
            get { return promotedMinUnitSalePrice.Value; }
        }

        public BitmapImage Icon
        {
            get { return Promotion.Promoted.IconPng.ToImage(); }
        }

        // Due KeyValuePair not being able to report changes, we will have to create two separate lists
        private ReactiveList<int> ingredientsQuantities;

        public Dictionary<ItemBundledEntity, int> Ingredients
        {
            get
            {
                return Promotion.IngredientsEntities.Zip(ingredientsQuantities, (k, v) => new { k, v })
                    .ToDictionary(x => x.k, x => x.v);
            }
        }
    }
}
