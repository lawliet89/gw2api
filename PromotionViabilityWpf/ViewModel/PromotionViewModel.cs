using System;
using System.Collections.Generic;
using System.Linq;
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
            IngredientsQuantity =
                new ReactiveList<KeyValuePair<int, int>>(
                    promotion.Ingredients.Select(pair => new KeyValuePair<int, int>(pair.Key.Identifier, 0)))
                {
                    ChangeTrackingEnabled = true
                };

            IngredientsQuantity.ItemChanged
                        .Select(_ => promotion
                            .CostOfIngredients(IngredientsQuantity.ToDictionary(pair => pair.Key, pair => pair.Value)))
                        .ToProperty(this, x => x.Cost, out cost);

            this.WhenAnyValue(x => x.Promotion.Promoted.MinSaleUnitPrice)
                .ToProperty(this, x => x.PromotedMinSalePrice, out promotedMinSalePrice);

            Observable.Merge(new[]
            {
                this.WhenAnyValue(x => x.QuantityYield),
                this.WhenAnyValue(x => x.PromotedMinSalePrice).Select(_ => QuantityYield)
            })
                .Select(q => promotion.ProfitOfProduct(Convert.ToInt32(q)))
                .ToProperty(this, x => x.Profit, out profit);

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

        public double? QuantityYield
        {
            get { return quantityYield; }
            set { this.RaiseAndSetIfChanged(ref quantityYield, Convert.ToInt32(value)); }
        }

        public ReactiveList<KeyValuePair<int, int>> IngredientsQuantity { private set; get; }
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ObservableAsPropertyHelper<Coin> profit;

        public Coin Profit
        {
            get { return profit.Value; }
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
        private ObservableAsPropertyHelper<Coin> promotedMinSalePrice;

        public Coin PromotedMinSalePrice
        {
            get { return promotedMinSalePrice.Value; }
        }

        public BitmapImage Icon
        {
            get { return Promotion.Promoted.IconPng.ToImage(); }
        }
    }
}
