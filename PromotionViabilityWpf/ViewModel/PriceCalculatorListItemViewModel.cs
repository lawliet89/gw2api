using System.Reactive.Linq;
using gw2api.Model;
using ReactiveUI;

namespace PromotionViabilityWpf.ViewModel
{
    public class PriceCalculatorListItemViewModel : ReactiveObject
    {
        private readonly ItemBundledEntity item;

        public int Identifier
        {
            get { return item.Identifier; }
        }

        private readonly ObservableAsPropertyHelper<string> name;

        public string Name
        {
            get { return name.Value; }
        }
 

        public PriceCalculatorListItemViewModel(ItemBundledEntity item)
        {
            this.item = item;

            this.WhenAnyValue(x => x.item.Item)
                .Select(i => i.Name)
                .ToProperty(this, x => x.Name, out name);
        }
    }
}
