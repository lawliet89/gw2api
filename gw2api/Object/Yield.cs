namespace gw2api.Object
{
    public class Yield
    {
        public int Upper { get; set; }
        public int Lower { get; set; }
        public int? Average { get; set; } // Can be unknown, then it's null

        // Some useful data, sourced from GW2 Wiki
        public static Yield FineMaterialsPromotionYielld = new Yield()
        {
            Lower = 7,
            Upper = 40,
            Average = 18
        };

        public static Yield FineMaterialTier6PromotionYield = new Yield()
        {
            Lower = 5,
            Upper = 12,
            Average = 6
        };

        public static Yield DustPromotionYield = new Yield()
        {
            Lower = 40,
            Upper = 200
        };

        public static Yield CrystallineDustPromotionYield = new Yield()
        {
            Lower = 6,
            Upper = 40
        };
    }
}
