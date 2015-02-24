using System.Collections.Generic;
using PromotionViabilityWpf.Model;
using ReactiveUI;

namespace PromotionViabilityWpf.ViewModel
{
    public class PriceCalculatorViewModel : ReactiveObject
    {
        public List<ItemBundledEntity> Items { get; set; }

        public PriceCalculatorViewModel()
        {
        }
    }
}
