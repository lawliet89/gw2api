using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using gw2api.Object;
using GW2NET.Commerce;
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
            
            // FIXME: Won't get update notifications
            IngredientsQuantity =
                new ReactiveList<KeyValuePair<int, int>>(
                    promotion.Ingredients.Select(pair => new KeyValuePair<int, int>(pair.Key.Identifier, 0)))
                {
                    ChangeTrackingEnabled = true
                };

            var ingredientsObservables = new[]
            {
                IngredientsQuantity.ItemChanged.Select(_ => Unit.Default),
                IngredientsQuantity.ShouldReset.Select(_ => Unit.Default)
            };
            Observable.Merge(ingredientsObservables)
                        .Select(_ => promotion
                            .CostOfIngredients(IngredientsQuantity.ToDictionary(pair => pair.Key, pair => pair.Value)))
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
            CheckPopulated();
        }

        public void CheckPopulated()
        {
            Populated = Promotion.Populated;
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

        public ReactiveList<KeyValuePair<int, int>> IngredientsQuantity { private set; get; }
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

        private bool populated;

        public bool Populated
        {
            get { return populated; }
            private set { this.RaiseAndSetIfChanged(ref populated, value); }
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
    }
}
