using PromotionViabilityWpf.ViewModel;
using ReactiveUI;

namespace PromotionViabilityWpf.View
{
    /// <summary>
    /// Interaction logic for PriceCalculatorItemView.xaml
    /// </summary>
    public partial class PriceCalculatorItemView : IViewFor<PriceCalculatorItemViewModel>
    {
        public PriceCalculatorItemView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Icon, x => x.Icon.Source);
            this.OneWayBind(ViewModel, vm => vm.UnitBuyOfferPrice, x => x.UnitBuyOfferPrice.Content);
            this.OneWayBind(ViewModel, vm => vm.UnitSellOfferPrice, x => x.UnitSellOfferPrice.Content);
            this.Bind(ViewModel, vm => vm.Quantity, x => x.Quantity.Value);
            this.OneWayBind(ViewModel, vm => vm.BuyOfferPrice, x => x.BuyOfferPrice.Content);
            this.OneWayBind(ViewModel, vm => vm.SellOfferPrice, x => x.SellOfferPrice.Content);
            this.OneWayBind(ViewModel, vm => vm.BuyOfferPriceTaxed, x => x.BuyOfferPriceTaxed.Content);
            this.OneWayBind(ViewModel, vm => vm.SellOfferPriceTaxed, x => x.SellOfferPriceTaxed.Content);
            this.OneWayBind(ViewModel, vm => vm.Item.Item.Name, x => x.Expander.Header);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PriceCalculatorItemViewModel) value; }
        }

        public PriceCalculatorItemViewModel ViewModel { get; set; }
    }
}
