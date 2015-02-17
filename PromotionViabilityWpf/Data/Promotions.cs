using System.Collections.Generic;
using gw2api.Object;
using PromotionViabilityWpf.Model;

namespace PromotionViabilityWpf.Data
{
    internal class Promotions
    {
        public static List<Promotion> FineMaterialsTier6Promotions = new List<Promotion>
        {
            // Dust
            new Promotion
                (new ItemBundledEntity(ItemIds.CrystallineDust),
                    quantityYield: Yield.CrystallineDustPromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.IncandescentDust), 250},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 1}
                    }
                ),
            // Ancient bone
            new Promotion
                (new ItemBundledEntity(ItemIds.AncientBone),
                    quantityYield: Yield.FineMaterialTier6PromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.AncientBone), 1},
                        {new ItemBundledEntity(ItemIds.LargeBone), 50},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                    }
                ),
            // Claws
            new Promotion
                (new ItemBundledEntity(ItemIds.ViciousClaw),
                    quantityYield: Yield.FineMaterialTier6PromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.ViciousClaw), 1},
                        {new ItemBundledEntity(ItemIds.LargeClaws), 50},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                    }
                ),
            // Fangs
            new Promotion
                (new ItemBundledEntity(ItemIds.ViciousFang),
                    quantityYield: Yield.FineMaterialTier6PromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.ViciousFang), 1},
                        {new ItemBundledEntity(ItemIds.LargeFang), 50},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                    }
                ),
            // Scales
            new Promotion
                (new ItemBundledEntity(ItemIds.ArmoredScale),
                    quantityYield: Yield.FineMaterialTier6PromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.ArmoredScale), 1},
                        {new ItemBundledEntity(ItemIds.LargeScale), 50},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                    }
                ),
            // Totems
            new Promotion
                (new ItemBundledEntity(ItemIds.ElaborateTotem),
                    quantityYield: Yield.FineMaterialTier6PromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.ElaborateTotem), 1},
                        {new ItemBundledEntity(ItemIds.IntricateTotem), 50},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                    }
                ),
            // Venom sacs
            new Promotion
                (new ItemBundledEntity(ItemIds.PowerfulVenomSac),
                    quantityYield: Yield.FineMaterialTier6PromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.PowerfulVenomSac), 1},
                        {new ItemBundledEntity(ItemIds.PotentVenomSac), 50},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                    }
                ),
            // Blood
            new Promotion
                (new ItemBundledEntity(ItemIds.PowerfulBlood),
                    quantityYield: Yield.FineMaterialTier6PromotionYield,
                    ingredients: new Dictionary<ItemBundledEntity, int>
                    {
                        {new ItemBundledEntity(ItemIds.PowerfulBlood), 1},
                        {new ItemBundledEntity(ItemIds.PotentBlood), 50},
                        {new ItemBundledEntity(ItemIds.CrystallineDust), 5}
                    }
                )
        };
    }
}
