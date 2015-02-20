using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using PromotionViabilityWpf.Data;
using PromotionViabilityWpf.Model;
using PromotionViabilityWpf.ViewModel;
using ReactiveUI;

namespace PromotionViabilityWpf.View
{
    /// <summary>
    /// Interaction logic for PriceCalculator.xaml
    /// </summary>
    public partial class PriceCalculator : IViewFor<PriceCalculatorViewModel>
    {
        public IEnumerable<ItemBundledEntity> Items { get; set; }

        public PriceCalculator()
        {
            InitializeComponent();
        }

        public override void BeginInit()
        {
        }

        public override void EndInit()
        {
            ViewModel = new PriceCalculatorViewModel(Items);
            this.OneWayBind(ViewModel, vm => vm.Items, x => x.ItemList.ItemsSource, l => l.Select(i => i.Item.Name));
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PriceCalculatorViewModel) value; }
        }

        public PriceCalculatorViewModel ViewModel { get; set; }
    }
}
