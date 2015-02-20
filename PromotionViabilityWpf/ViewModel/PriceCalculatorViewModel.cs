using System.Collections.Generic;
using PromotionViabilityWpf.Model;
using ReactiveUI;

namespace PromotionViabilityWpf.ViewModel
{
    public class PriceCalculatorViewModel : ReactiveObject
    {
        public ReactiveList<ItemBundledEntity> Items { get; private set; }

        public PriceCalculatorViewModel(IEnumerable<ItemBundledEntity> items)
        {
            Items = new ReactiveList<ItemBundledEntity>(items);
        }
    }
}
