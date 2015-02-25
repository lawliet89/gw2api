using System;
using GW2NET;
using GW2NET.Commerce;
using GW2NET.Common;
using GW2NET.Items;
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
            Locator.CurrentMutable.RegisterConstant(GW2.V2.Items.ForCurrentCulture(), typeof(IRepository<int, Item>));
            Locator.CurrentMutable.RegisterConstant(GW2.V2.Commerce.Prices, typeof(IRepository<int, AggregateListing>));
            Locator.CurrentMutable.RegisterConstant(GW2.Rendering.RenderService, typeof(IRenderService));
            Locator.CurrentMutable.Register(() => new PromotionView(), typeof(IViewFor<PromotionViewModel>));
            Locator.CurrentMutable.RegisterConstant(new IntegerToNullableDouble(), typeof(IBindingTypeConverter));
            Locator.CurrentMutable.RegisterConstant(new DebugLogger(), typeof(ILogger));
            App.Main();
        }
    }
}