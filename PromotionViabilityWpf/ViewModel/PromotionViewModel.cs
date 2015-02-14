﻿using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using gw2api.Model;
using gw2api.Object;
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
            this.WhenAnyValue(x => x.QuantityYield)
                .Select(promotion.ProfitOfProduct)
                .ToProperty(this, x => x.Profit, out profit);
            this.WhenAnyValue(x => x.Promotion)
                .Select(x => x.Populated)
                .ToProperty(this, x => x.Populated, out populated);
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

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ObservableAsPropertyHelper<bool> populated;

        public bool Populated
        {
            get { return populated.Value; }
        }
    }
}
