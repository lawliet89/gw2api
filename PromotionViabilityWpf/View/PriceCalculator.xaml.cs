using PromotionViabilityWpf.Extensions;
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

            //this.OneWayBind(ViewModel, vm => vm.SelectedIcon, x => x.Icon.Source);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PriceCalculatorViewModel) value; }
        }

        public PriceCalculatorViewModel ViewModel { get; set; }
    }
}
