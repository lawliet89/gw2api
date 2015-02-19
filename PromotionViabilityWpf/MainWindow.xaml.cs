using System.Diagnostics;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
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
        public MainWindow()
        {
            InitializeComponent();
            
#if !DEBUG
            BreakButton.Visibility = Visibility.Hidden;;
#endif

            ViewModel = new MainWindowViewModel();
            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.LoadingIndicator.Visibility);
            this.OneWayBind(ViewModel, vm => vm.LoadedPromotions, x => x.PromotionList.ItemsSource);

            this.BindCommand(ViewModel, vm => vm.RefreshCommand, x => x.RefreshButton, "Click");

            var serializer = JsonSerializer.CreateDefault();
            serializer.Formatting = Formatting.Indented;
            using (var streamWriter = new StreamWriter("test.json"))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    var promotions = Promotions.FineMaterialsTier6Promotions;
                    serializer.Serialize(jsonWriter, promotions);
                }
            }
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
