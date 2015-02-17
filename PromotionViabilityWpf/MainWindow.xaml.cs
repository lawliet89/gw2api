using System.Diagnostics;
using System.Windows;
using PromotionViabilityWpf.ViewModel;
using ReactiveUI;

namespace PromotionViabilityWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IViewFor<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();
            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.LoadingIndicator.Visibility);
            this.OneWayBind(ViewModel, vm => vm.LoadedPromotions, x => x.PromotionList.ItemsSource);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as MainWindowViewModel; }
        }

        public MainWindowViewModel ViewModel { get; set; }

        public void DebuggerBreak(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}
