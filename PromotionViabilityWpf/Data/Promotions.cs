using System.Collections.Generic;
using gw2api.Model;
using gw2api.Object;

namespace PromotionViabilityWpf.Data
{
    internal static class Promotions
    {
        public static List<Promotion> FineMaterialsTier6Promotions { get; private set; }

        internal static void CreatePromotions()
        {
            FineMaterialsTier6Promotions = new List<Promotion>
            {
                // Dust
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust),
                        quantityYield: Yield.CrystallineDustPromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.IncandescentDust), 250},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 1}
                        }
                    ),
                // Ancient bone
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.AncientBone),
                        quantityYield: Yield.FineMaterialTier6PromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.AncientBone), 1},
                            {ItemBundledEntity.CreateOrGet(ItemIds.LargeBone), 50},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 5}
                        }
                    ),
                // Claws
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.ViciousClaw),
                        quantityYield: Yield.FineMaterialTier6PromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.ViciousClaw), 1},
                            {ItemBundledEntity.CreateOrGet(ItemIds.LargeClaws), 50},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 5}
                        }
                    ),
                // Fangs
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.ViciousFang),
                        quantityYield: Yield.FineMaterialTier6PromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.ViciousFang), 1},
                            {ItemBundledEntity.CreateOrGet(ItemIds.LargeFang), 50},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 5}
                        }
                    ),
                // Scales
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.ArmoredScale),
                        quantityYield: Yield.FineMaterialTier6PromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.ArmoredScale), 1},
                            {ItemBundledEntity.CreateOrGet(ItemIds.LargeScale), 50},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 5}
                        }
                    ),
                // Totems
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.ElaborateTotem),
                        quantityYield: Yield.FineMaterialTier6PromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.ElaborateTotem), 1},
                            {ItemBundledEntity.CreateOrGet(ItemIds.IntricateTotem), 50},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 5}
                        }
                    ),
                // Venom sacs
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.PowerfulVenomSac),
                        quantityYield: Yield.FineMaterialTier6PromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.PowerfulVenomSac), 1},
                            {ItemBundledEntity.CreateOrGet(ItemIds.PotentVenomSac), 50},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 5}
                        }
                    ),
                // Blood
                Promotion.GetOrCreate
                    (ItemBundledEntity.CreateOrGet(ItemIds.PowerfulBlood),
                        quantityYield: Yield.FineMaterialTier6PromotionYield,
                        ingredients: new Dictionary<ItemBundledEntity, int>
                        {
                            {ItemBundledEntity.CreateOrGet(ItemIds.PowerfulBlood), 1},
                            {ItemBundledEntity.CreateOrGet(ItemIds.PotentBlood), 50},
                            {ItemBundledEntity.CreateOrGet(ItemIds.CrystallineDust), 5}
                        }
                    )
            };
        }
    }
}
