using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using gw2api.Extension;
using PromotionViabilityWpf.Extensions;
using PromotionViabilityWpf.ViewModel;
using ReactiveUI;
using Splat;

namespace PromotionViabilityWpf.View
{
    /// <summary>
    /// Interaction logic for PromotionView.xaml
    /// </summary>
    public partial class PromotionView : IViewFor<PromotionViewModel>, IEnableLogger
    {
        public PromotionView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Name, x => x.PromotedName.Text);
            this.OneWayBind(ViewModel, vm => vm.Icon, x => x.Icon.Source);

            this.Bind(ViewModel, vm => vm.QuantityYield, x => x.QuantityYield.Value);
            this.OneWayBind(ViewModel, vm => vm.Promotion.QuantityYield.Upper, x => x.QuantityYield.Maximum);
            this.OneWayBind(ViewModel, vm => vm.Promotion.QuantityYield.Lower, x => x.QuantityYield.Minimum);
            this.OneWayBind(ViewModel, vm => vm.RevenueOfProduct, x => x.ProfitOfProduct.Text);

            this.OneWayBind(ViewModel, vm => vm.Ingredients, x => x.Ingredients.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.IngredientsCost, x => x.IngredientsCost.Text);
            this.OneWayBind(ViewModel, vm => vm.IngredientsValue, x => x.IngredientsValue.Text);

            this.OneWayBind(ViewModel, vm => vm.Profit, x => x.Profit.Text);

            this.WhenAnyValue(x => x.ViewModel.Profit)
                .Subscribe(profit =>
                {
                    Style = profit > 0
                        ? (Style) FindResource("PositivePromotion")
                        : (Style) FindResource("NegativePromotion");
                });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as PromotionViewModel; }
        }

        public PromotionViewModel ViewModel { get; set; }

        // Credits to http://wpf.codeplex.com/wikipage?title=Single-Click%20Editing
        private void EnterEditMode(object sender, MouseEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row == null) return;
            if (row.IsEditing) return;

            var cell = row.GetChildrenOfType<DataGridCell>().First(c => !c.IsReadOnly);

            cell.Focus();
            row.IsSelected = true;
            cell.IsEditing = true;

            var grid = row.FindVisualParent<DataGrid>();
            if (grid == null) return;
        }

        private void LeaveEditMode(object sender, MouseEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row == null) return;

            var grid = row.FindVisualParent<DataGrid>();
            if (grid == null) return;

            row.GetChildrenOfType<DataGridCell>().Where(c => c.IsEditing)
                .ForEach(c => c.IsEditing = false);
        }
    }
}
