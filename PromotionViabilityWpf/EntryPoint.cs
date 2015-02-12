using System;
using GW2NET.Commerce;
using GW2NET.Items;
using PromotionViabilityWpf.ViewModel;

namespace PromotionViabilityWpf
{
    public class EntryPoint
    {
        public static Loader Loader;

        [STAThread]
        public static void Main(string[] args)
        {
            Loader = new Loader();
            App.Main();

            Loader.Load<int, Item>(Promotions.FineMaterialsTier6Promotions);
            Loader.Load<int, AggregateListing>(Promotions.FineMaterialsTier6Promotions);
        }
    }
}