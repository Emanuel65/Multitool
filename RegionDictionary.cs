using System.Collections.Generic;

namespace Multitool
{
    /// <summary>
    /// The dictionary constructor for the region data
    /// It derives from Dictionary class so that in has the same functionality
    /// </summary>
    class RegionDictionary : Dictionary<int, RegionMembers>
    {
        // Overrides the Add method and takes in 3 arguments
        public void Add(int key, int brpCode, string regionName)
        {
            // Creates a new instance of the struct with the data
            RegionMembers rgData = new RegionMembers()
            {
                // Initializes the instance 
                // with the values passed in as parameters
                BrpCode = brpCode,
                RegionName = regionName
            };
            // Calls the add method and adds the data directly
            Add(key, rgData);

        }
    }
}
