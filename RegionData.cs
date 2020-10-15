using System.Collections.Generic;

namespace Multitool
{
    class RegionData
    {
        public int gstCode { get; private set; }
        public int brpCode { get; private set; }
        public string regionName { get; private set; }

        private static RegionData region1 = new RegionData() { gstCode = 21, brpCode = 51, regionName = "Region1" };
        private static RegionData region2 = new RegionData() { gstCode = 22, brpCode = 52, regionName = "Region2" };
        private static RegionData region3 = new RegionData() { gstCode = 23, brpCode = 53, regionName = "Region3" };
        private static RegionData region4 = new RegionData() { gstCode = 24, brpCode = 54, regionName = "Region4" };
        private static RegionData region5 = new RegionData() { gstCode = 25, brpCode = 55, regionName = "Region5" };

        public static Dictionary<int, RegionData> regionDictionary = new Dictionary<int, RegionData>
        {
            { region1.gstCode, region1 },
            { region2.gstCode, region2 },
            { region3.gstCode, region3 },
            { region4.gstCode, region4 },
            { region5.gstCode, region5 }
        };

    }

}
