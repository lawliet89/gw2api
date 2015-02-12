using GW2NET.Commerce;
using GW2NET.Items;
using PromotionViabilityWpf.Data;
using PromotionViabilityWpf.ViewModel;
using ReactiveUI;

namespace PromotionViabilityWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IViewFor<MainWindowViewModel>
    {
        private MainWindowViewModel MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel = new MainWindowViewModel();
            MainWindowViewModel.Load<int, Item>(Promotions.FineMaterialsTier6Promotions);
            MainWindowViewModel.Load<int, AggregateListing>(Promotions.FineMaterialsTier6Promotions);

            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.LoadingIndicator.Visibility);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as MainWindowViewModel; }
        }

        public MainWindowViewModel ViewModel
        {
            get { return MainWindowViewModel; }
            set { MainWindowViewModel = value; }
        }
    }
}
