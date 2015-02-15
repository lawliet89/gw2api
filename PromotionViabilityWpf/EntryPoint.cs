using System;
using PromotionViabilityWpf.Converter;
using PromotionViabilityWpf.View;
using PromotionViabilityWpf.ViewModel;
using ReactiveUI;
using Splat;

namespace PromotionViabilityWpf
{
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Locator.CurrentMutable.Register(() => new PromotionView(), typeof(IViewFor<PromotionViewModel>));
            Locator.CurrentMutable.RegisterConstant(new IntegerToNullableDouble(), typeof(IBindingTypeConverter));
            Locator.CurrentMutable.RegisterConstant(new DebugLogger(), typeof(ILogger));
            App.Main();
        }
    }
}