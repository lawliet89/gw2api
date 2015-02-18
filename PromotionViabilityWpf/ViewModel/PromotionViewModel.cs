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
                ingredientsQuantities.ItemChanged.Select(_ => Unit.Default),
                ingredientsQuantities.ShouldReset.Select(_ => Unit.Default),
                Promotion.IngredientsEntities.ItemChanged.Select(_ => Unit.Default),
                Promotion.IngredientsEntities.ShouldReset.Select(_ => Unit.Default),
            };
            Observable.Merge(ingredientsObservables)
                .Where(_ => Promotion.Populated)
                .Subscribe(_ => Cost = promotion.CostOfIngredients(Ingredients));

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

        private readonly ObservableAsPropertyHelper<Coin> profitOfProduct;

        public Coin ProfitOfProduct
        {
            get { return profitOfProduct.Value; }
        }

        private Coin cost;

        public Coin Cost
        {
            get { return cost; }
            private set { this.RaiseAndSetIfChanged(ref cost, value); }
        }

        private readonly ObservableAsPropertyHelper<bool> populated;

        public bool Populated
        {
            get { return populated.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> promotedMinUnitSalePrice;

        public Coin PromotedMinUnitSalePrice
        {
            get { return promotedMinUnitSalePrice.Value; }
        }

        public BitmapImage Icon
        {
            get { return Promotion.Promoted.IconPng.ToImage(); }
        }

        // Due KeyValuePair not being able to report changes, we will have to create two separate lists
        private readonly ReactiveList<int> ingredientsQuantities;

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
