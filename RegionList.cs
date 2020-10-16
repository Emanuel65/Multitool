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
            var list = new RegionDictionary();
            list.Add(35, 26, "Andaman and Nicobar Islands");
            list.Add(37, 36, "Andhra Pradesh(New)");
            list.Add(12, 2, "Arunachal Pradesh");
            list.Add(18, 3, "Assam");
            list.Add(10, 4, "Bihar");
            list.Add(4, 27, "Chandigarh");
            list.Add(22, 33, "Chattisgarh");
            list.Add(26, 28, "Dadra and Nagar Haveli");
            list.Add(25, 29, "Daman and Diu");
            list.Add(7, 30, "Delhi");
            list.Add(30, 5, "Goa");
            list.Add(24, 6, "Gujarat");
            list.Add(6, 7, "Haryana");
            list.Add(2, 8, "Himachal Pradesh");
            list.Add(1, 9, "Jammu and Kashmir");
            list.Add(20, 34, "Jharkhand");
            list.Add(29, 10, "Karnataka");
            list.Add(32, 11, "Kerala");
            list.Add(31, 31, "Lakshadweep Islands");
            list.Add(23, 12, "Madhya Pradesh");
            list.Add(27, 13, "Maharashtra");
            list.Add(14, 14, "Manipur");
            list.Add(17, 15, "Meghalaya");
            list.Add(15, 16, "Mizoram");
            list.Add(13, 17, "Nagaland");
            list.Add(21, 18, "Odisha");
            list.Add(34, 32, "Pondicherry");
            list.Add(3, 19, "Punjab");
            list.Add(8, 20, "Rajasthan");
            list.Add(11, 21, "Sikkim");
            list.Add(33, 22, "Tamil Nadu");
            list.Add(36, 37, "Telangana");
            list.Add(16, 23, "Tripura");
            list.Add(9, 24, "Uttar Pradesh");
            list.Add(5, 35, "Uttarakhand");
            list.Add(19, 25, "West Bengal");

            return list;

        }
    }
}
