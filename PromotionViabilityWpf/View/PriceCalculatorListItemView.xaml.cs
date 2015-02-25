using PromotionViabilityWpf.ViewModel;
using ReactiveUI;

namespace PromotionViabilityWpf.View
{
    /// <summary>
    /// Interaction logic for PriceCalculatorListItemView.xaml
    /// </summary>
    public partial class PriceCalculatorListItemView : IViewFor<PriceCalculatorListItemViewModel>
    {
        public PriceCalculatorListItemView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Name, x => x.ItemName.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PriceCalculatorListItemViewModel) value; }
        }

        public PriceCalculatorListItemViewModel ViewModel { get; set; }
    }
}
