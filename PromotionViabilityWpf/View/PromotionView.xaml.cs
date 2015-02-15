﻿using PromotionViabilityWpf.Extensions;
using PromotionViabilityWpf.ViewModel;
using ReactiveUI;

namespace PromotionViabilityWpf.View
{
    /// <summary>
    /// Interaction logic for PromotionView.xaml
    /// </summary>
    public partial class PromotionView : IViewFor<PromotionViewModel>
    {
        public PromotionView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Name, x => x.PromotedName.Text);
            this.OneWayBind(ViewModel, vm => vm.Icon, x => x.Icon.Source);

            this.Bind(ViewModel, vm => vm.QuantityYield, x => x.QuantityYield.Value);
            this.OneWayBind(ViewModel, vm => vm.Promotion.QuantityYield.Upper, x => x.QuantityYield.Maximum);
            this.OneWayBind(ViewModel, vm => vm.Promotion.QuantityYield.Lower, x => x.QuantityYield.Minimum);
            this.OneWayBind(ViewModel, vm => vm.ProfitOfProduct, x => x.ProfitOfProduct.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as PromotionViewModel; }
        }

        public PromotionViewModel ViewModel { get; set; }
    }
}
