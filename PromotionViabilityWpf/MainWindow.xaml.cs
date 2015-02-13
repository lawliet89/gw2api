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
        private MainWindowViewModel MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel = new MainWindowViewModel();
            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.LoadingIndicator.Visibility);
            this.OneWayBind(ViewModel, vm => vm.LoadedPromotions, x => x.PromotionList.ItemsSource);
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

        public void DebuggerBreak(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}
