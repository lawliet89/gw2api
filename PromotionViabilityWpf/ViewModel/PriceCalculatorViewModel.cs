using System.Collections.Generic;
using gw2api.Model;
using ReactiveUI;

namespace PromotionViabilityWpf.ViewModel
{
    public class PriceCalculatorViewModel : ReactiveObject
    {
        public List<ItemBundledEntity> Items { get; private set; }

        public PriceCalculatorViewModel()
        {
            Items = new List<ItemBundledEntity>();
        }
    }
}
