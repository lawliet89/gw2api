using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PromotionViability
{
    public static class ItemIds
    {
        public static int CrystallineDust = 24277;
        public static int IncandescentDust = 24276;

        public static int AncientBone = 24358;
        public static int LargeBone = 24341;

        public static int ViciousClaw = 24351;
        public static int LargeClaws = 24350;

        public static int ViciousFang = 24357;
        public static int LargeFang = 24356;

        public static int ArmoredScale = 24289;
        public static int LargeScale = 24288;

        public static int IntricateTotem = 24299;
        public static int ElaborateTotem = 24300;

        public static int PotentVenomSac = 24282;
        public static int PowerfulVenomSac = 24283;

        public static int PotentBlood = 24294;
        public static int PowerfulBlood = 24295;

        public static IEnumerable<int> AllIds()
        {
            return typeof(ItemIds).GetFields()
                .Where(f => f.IsPublic && f.IsStatic && f.FieldType == typeof (int))
                .Select(f => f.GetValue(null))
                .OfType<int>();
        }
    }
}