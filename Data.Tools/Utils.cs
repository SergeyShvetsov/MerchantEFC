using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Tools
{
    public static class Utils
    {
        public static string GetPriceString(double? minPrice, double? maxPrice)
        {
            var res = string.Empty;
            if (minPrice == null) return res;
            if (minPrice == maxPrice) return ((double)maxPrice).ToString("N2");
            return string.Format("{0:N2} - {1:N2}", minPrice, maxPrice);
        }
    }
}
