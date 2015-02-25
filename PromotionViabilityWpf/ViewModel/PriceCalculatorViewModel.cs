using System.Collections.Generic;
using gw2api.Model;
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
