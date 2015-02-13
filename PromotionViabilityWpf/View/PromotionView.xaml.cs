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

            this.OneWayBind(ViewModel, vm => vm.Name, x => x.Name.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as PromotionViewModel; }
        }

        public PromotionViewModel ViewModel { get; set; }
    }
}
