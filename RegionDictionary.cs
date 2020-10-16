using System.Collections.Generic;

namespace Multitool
{
    class RegionDictionary : Dictionary<int, RegionData>
    {
        public void Add(int key, int brpCode, string regionName)
        {
            RegionData rgData = new RegionData();
            rgData.BrpCode = brpCode;
            rgData.RegionName = regionName;
            Add(key, rgData);
        }
    }
}
