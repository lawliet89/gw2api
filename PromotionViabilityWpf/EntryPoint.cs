using System;
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
            App.Main();
        }
    }
}