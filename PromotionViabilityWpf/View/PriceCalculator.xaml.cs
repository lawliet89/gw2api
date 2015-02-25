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
        public static readonly DependencyProperty ItemsListProperty = DependencyProperty.Register("ItemsList",
            typeof(List<ItemBundledEntity>), typeof(PriceCalculator));

        public List<ItemBundledEntity> ItemsList
        {
            get { return (List<ItemBundledEntity>)GetValue(ItemsListProperty); }
            set {  SetValue(ItemsListProperty, value);}
        }

        public PriceCalculator()
        {
            InitializeComponent();
            ViewModel = new PriceCalculatorViewModel();

            this.Bind(ViewModel, vm => vm.Items, x => x.ItemsList);
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
