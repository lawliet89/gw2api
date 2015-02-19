using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace gw2api.Object
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Yield
    {
        [JsonProperty]
        public int Upper { get; set; }
        [JsonProperty]
        public int Lower { get; set; }
        [JsonProperty("Average")]
        private int? average;

        public int Average
        {
            get { return average ?? CalculateAverage(); }
            set { average = value; }
        } // Can be unknown, then it's null

        public int CalculateAverage()
        {
            return (Upper + Lower)/2;
        }

        public IEnumerable<int> Range
        {
            get { return Enumerable.Range(Lower, Upper); }
        } 

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
