using System.Collections.Generic;
using gw2api.Object;
using PromotionViabilityWpf.Model;

namespace PromotionViabilityWpf.Data
{
    class Promotions
    {
        public static List<Promotion> FineMaterialsTier6Promotions = new List<Promotion>()
        {
            // Dust
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.CrystallineDust),
                QuantityYield = Yield.CrystallineDustPromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.IncandescentDust), 250},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 1}
                }
            },
            // Ancient bone
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.AncientBone),
                QuantityYield = Yield.FineMaterialTier6PromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.AncientBone), 1},
                    {new ItemBundledEntity(ItemIds.LargeBone), 50},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                }
            },
            // Claws
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.ViciousClaw),
                QuantityYield = Yield.FineMaterialTier6PromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.ViciousClaw), 1},
                    {new ItemBundledEntity(ItemIds.LargeClaws), 50},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                }
            },
            // Fangs
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.ViciousFang),
                QuantityYield = Yield.FineMaterialTier6PromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.ViciousFang), 1},
                    {new ItemBundledEntity(ItemIds.LargeFang), 50},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                }
            },
            // Scales
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.ArmoredScale),
                QuantityYield = Yield.FineMaterialTier6PromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.ArmoredScale), 1},
                    {new ItemBundledEntity(ItemIds.LargeScale), 50},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                }
            },
            // Totems
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.ElaborateTotem),
                QuantityYield = Yield.FineMaterialTier6PromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.ElaborateTotem), 1},
                    {new ItemBundledEntity(ItemIds.IntricateTotem), 50},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                }
            },
            // Venom sacs
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.PowerfulVenomSac),
                QuantityYield = Yield.FineMaterialTier6PromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.PowerfulVenomSac), 1},
                    {new ItemBundledEntity(ItemIds.PotentVenomSac), 50},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                }
            },
            // Blood
            new Promotion
            {
                Promoted = new ItemBundledEntity(ItemIds.PowerfulBlood),
                QuantityYield = Yield.FineMaterialTier6PromotionYield,
                Ingredients = new Dictionary<ItemBundledEntity, int>
                {
                    {new ItemBundledEntity(ItemIds.PowerfulBlood), 1},
                    {new ItemBundledEntity(ItemIds.PotentBlood), 50},
                    {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                }
            }
        };
    }
}
