using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using gw2api.Object;
using gw2api.Request;
using GW2NET;
using GW2NET.Commerce;
using GW2NET.Items;

namespace PromotionViability
{
    class Program
    {
        public static List<Promotion> Promotions = new List<Promotion>()
        {
            // Dust
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.CrystallineDust),
                QuantityYield = 23,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.IncandescentDust), 250},
                    {new ItemWrapper(ItemIds.CrystallineDust), 1}
                }
            },
            // Ancient bone
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.AncientBone),
                QuantityYield = Promotion.Tier5PromotionYield,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.AncientBone), 1},
                    {new ItemWrapper(ItemIds.LargeBone), 50},
                    {new ItemWrapper(ItemIds.CrystallineDust), 5}
                }
            },
            // Claws
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.ViciousClaw),
                QuantityYield = Promotion.Tier5PromotionYield,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.ViciousClaw), 1},
                    {new ItemWrapper(ItemIds.LargeClaws), 50},
                    {new ItemWrapper(ItemIds.CrystallineDust), 5}
                }
            },
            // Fangs
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.ViciousFang),
                QuantityYield = Promotion.Tier5PromotionYield,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.ViciousFang), 1},
                    {new ItemWrapper(ItemIds.LargeFang), 50},
                    {new ItemWrapper(ItemIds.CrystallineDust), 5}
                }
            },
            // Scales
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.ArmoredScale),
                QuantityYield = Promotion.Tier5PromotionYield,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.ArmoredScale), 1},
                    {new ItemWrapper(ItemIds.LargeScale), 50},
                    {new ItemWrapper(ItemIds.CrystallineDust), 5}
                }
            },
            // Totems
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.ElaborateTotem),
                QuantityYield = Promotion.Tier5PromotionYield,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.ElaborateTotem), 1},
                    {new ItemWrapper(ItemIds.IntricateTotem), 50},
                    {new ItemWrapper(ItemIds.CrystallineDust), 5}
                }
            },
            // Venom sacs
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.PowerfulVenomSac),
                QuantityYield = Promotion.Tier5PromotionYield,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.PowerfulVenomSac), 1},
                    {new ItemWrapper(ItemIds.PotentVenomSac), 50},
                    {new ItemWrapper(ItemIds.CrystallineDust), 5}
                }
            },
            // Blood
            new Promotion
            {
                Promoted = new ItemWrapper(ItemIds.PowerfulBlood),
                QuantityYield = Promotion.Tier5PromotionYield,
                Ingredients = new Dictionary<ItemWrapper, int>
                {
                    {new ItemWrapper(ItemIds.PowerfulBlood), 1},
                    {new ItemWrapper(ItemIds.PotentBlood), 50},
                    {new ItemWrapper(ItemIds.CrystallineDust), 5}
                }
            }
        };
        static void Main(string[] args)
        {
            var itemTask = Bundler.BundleAndSet<int, Item>(Promotions);
            var priceTask = Bundler.BundleAndSet<int, AggregateListing>(Promotions);

            var tasks = new[] {itemTask, priceTask};
            Console.WriteLine("Requesting data from GW2 API");

            while (!tasks.All(t => t.IsCompleted))
            {
                Console.Write(".");
                Thread.Sleep(500);
            }

            Console.WriteLine();
            foreach (var promotion in Promotions)
            {
                Console.WriteLine("Promoting to {0}:", promotion.Name);
                Console.WriteLine("\tCost of ingredients: {0}", promotion.CostOfIngridients);
                Console.WriteLine("\tProfit from selling an average yield of {0}: {1}",
                    promotion.QuantityYield, promotion.ProfitOfProduct);
                Console.WriteLine("\tProfit overall: {0}", promotion.ProfitOfPromotion);
                Console.WriteLine("\tVerdict: {0}", promotion.Profitable ? "Profitable" : "Don't bother");
            }

            // Keep the console window open in debug modde.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
