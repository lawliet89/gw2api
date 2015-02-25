using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using gw2api.Model;
using gw2api.Object;
using PromotionViabilityWpf.Extensions;
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

            Ingredients =
                new ReactiveList<IngredientViewModel>(
                    Promotion.Ingredients.Select(pair => new IngredientViewModel(pair.Key, pair.Value))) { ChangeTrackingEnabled = true };

            Cost = 0;
            var ingredientsObservables = new[]
            {
                Ingredients.ItemChanged.Select(_ => Unit.Default),
                Ingredients.ShouldReset.Select(_ => Unit.Default),
            };
            Observable.Merge(ingredientsObservables)
                .Where(_ => Promotion.Populated)
                .Subscribe(_ => Cost = promotion.CostOfIngredients(ingredientsList));

            this.WhenAnyValue(x => x.Promotion.Promoted.MinSaleUnitPrice)
                .ToProperty(this, x => x.PromotedMinUnitSalePrice, out promotedMinUnitSalePrice, 0);

            Observable.Merge(new[]
            {
                this.WhenAnyValue(x => x.QuantityYield),
                this.WhenAnyValue(x => x.PromotedMinUnitSalePrice).Select(_ => QuantityYield)
            })
                .Select(promotion.ProfitOfProduct)
                .ToProperty(this, x => x.RevenueOfProduct, out revenueOfProduct, 0);

            Observable.Merge(new[]
            {
                this.WhenAnyValue(x => x.Cost),
                this.WhenAnyValue(x => x.RevenueOfProduct)
            })
                .Select(_ => RevenueOfProduct - Cost)
                .ToProperty(this, x => x.Profit, out profit, 0);
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

        private readonly ObservableAsPropertyHelper<Coin> revenueOfProduct;

        public Coin RevenueOfProduct
        {
            get { return revenueOfProduct.Value; }
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


        public ReactiveList<IngredientViewModel> Ingredients;

        private Dictionary<ItemBundledEntity, int> ingredientsList
        {
            get
            {
                return Promotion.IngredientsEntities.Zip(Ingredients, (k, v) => new { k, v.Quantity })
                    .ToDictionary(x => x.k, x => x.Quantity);
            }
        }

        private readonly ObservableAsPropertyHelper<Coin> profit;

        public Coin Profit
        {
            get { return profit.Value; }
        }
    }
}
