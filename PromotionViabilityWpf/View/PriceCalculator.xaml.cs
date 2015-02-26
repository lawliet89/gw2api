using PromotionViabilityWpf.ViewModel;
using ReactiveUI;

namespace PromotionViabilityWpf.View
{
    /// <summary>
    /// Interaction logic for PriceCalculator.xaml
    /// </summary>
    public partial class PriceCalculator : IViewFor<PriceCalculatorViewModel>
    {
        public PriceCalculator()
        {
            InitializeComponent();
            ViewModel = new PriceCalculatorViewModel();

            this.OneWayBind(ViewModel, vm => vm.Items, x => x.ItemComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedItem, x => x.ItemComboBox.SelectedItem);

            this.OneWayBind(ViewModel, vm => vm.SelectedIcon, x => x.Icon.Source);
            this.OneWayBind(ViewModel, vm => vm.UnitBuyOfferPrice, x => x.UnitBuyOfferPrice.Content);
            this.OneWayBind(ViewModel, vm => vm.UnitSellOfferPrice, x => x.UnitSellOfferPrice.Content);
            this.Bind(ViewModel, vm => vm.Quantity, x => x.Quantity.Value);
            this.OneWayBind(ViewModel, vm => vm.BuyOfferPrice, x => x.BuyOfferPrice.Content);
            this.OneWayBind(ViewModel, vm => vm.SellOfferPrice, x => x.SellOfferPrice.Content);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PriceCalculatorViewModel) value; }
        }

        public PriceCalculatorViewModel ViewModel { get; set; }
    }
}
