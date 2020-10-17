namespace Multitool
{
    /// <summary>
    /// Simple class to hold a list of regions
    /// </summary>
    class RegionList
    {
        // Returns the modified version of the Dictionary class with all the values
        public static RegionDictionary ReturnRegionList()
        {
            var list = new RegionDictionary
            {
                { 35, 26, "Andaman and Nicobar Islands" },
                { 37, 36, "Andhra Pradesh(New)" },
                { 12, 2, "Arunachal Pradesh" },
                { 18, 3, "Assam" },
                { 10, 4, "Bihar" },
                { 4, 27, "Chandigarh" },
                { 22, 33, "Chattisgarh" },
                { 26, 28, "Dadra and Nagar Haveli" },
                { 25, 29, "Daman and Diu" },
                { 7, 30, "Delhi" },
                { 30, 5, "Goa" },
                { 24, 6, "Gujarat" },
                { 6, 7, "Haryana" },
                { 2, 8, "Himachal Pradesh" },
                { 1, 9, "Jammu and Kashmir" },
                { 20, 34, "Jharkhand" },
                { 29, 10, "Karnataka" },
                { 32, 11, "Kerala" },
                { 31, 31, "Lakshadweep Islands" },
                { 23, 12, "Madhya Pradesh" },
                { 27, 13, "Maharashtra" },
                { 14, 14, "Manipur" },
                { 17, 15, "Meghalaya" },
                { 15, 16, "Mizoram" },
                { 13, 17, "Nagaland" },
                { 21, 18, "Odisha" },
                { 34, 32, "Pondicherry" },
                { 3, 19, "Punjab" },
                { 8, 20, "Rajasthan" },
                { 11, 21, "Sikkim" },
                { 33, 22, "Tamil Nadu" },
                { 36, 37, "Telangana" },
                { 16, 23, "Tripura" },
                { 9, 24, "Uttar Pradesh" },
                { 5, 35, "Uttarakhand" },
                { 19, 25, "West Bengal" }
            };

            return list;

        }
    }
}
