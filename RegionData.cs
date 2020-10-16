namespace Multitool
{
    /// <summary>
    /// A struct to hold two values
    /// </summary>
    struct RegionData
    {
        // The BrpCode, the first value
        public int BrpCode { get; set; }

        // The Region Name, the second value
        public string RegionName { get; set; }
    }

    //***This works in a key value Dictionary
    // so that I can have two pieces of data asigned
    // to a single key.

}
