using System.Collections.Generic;
using System.Linq;
using System.Windows;
using gw2api.Model;
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

            this.OneWayBind(ViewModel, vm => vm.Items, x => x.ItemComboBox.ItemsSource, l => l.Select(i => i.Item.Name));
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PriceCalculatorViewModel) value; }
        }

        public PriceCalculatorViewModel ViewModel { get; set; }
    }
}
