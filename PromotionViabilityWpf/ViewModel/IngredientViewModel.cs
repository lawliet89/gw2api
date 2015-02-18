using System.Reactive.Linq;
using gw2api.Object;
using PromotionViabilityWpf.Model;
using ReactiveUI;

namespace PromotionViabilityWpf.ViewModel
{
    public class IngredientViewModel : ReactiveObject
    {
        private readonly ItemBundledEntity item;
        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { this.RaiseAndSetIfChanged(ref quantity, value); }
        }

        private readonly ObservableAsPropertyHelper<string> name;

        public string Name
        {
            get { return name.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> cost;

        public Coin Cost
        {
            get { return cost.Value; }
        }

        public IngredientViewModel(ItemBundledEntity item)
        {
            this.item = item;

            this.WhenAnyValue(x => x.item.Item)
                .Where(i => i != null)
                .Select(i => i.Name)
                .ToProperty(this, x => x.Name, out name);

            this.WhenAnyValue(x => x.item.Listings)
                .Where(l => l != null)
                .Select(l => (Coin) l.BuyOffers.UnitPrice)
                .ToProperty(this, x => x.Cost, out cost);

            Quantity = 0;
        }
    }
}
