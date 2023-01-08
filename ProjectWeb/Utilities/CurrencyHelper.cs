using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Utilities
{
    public class CurrencyHelper
    {
        public static string IntToNumeric(int price)
        {
            if (price == 0) return "0";

            System.Globalization.CultureInfo elGRs = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            return String.Format(elGRs, "{0:0,0}", Convert.ToDouble(price));
        }

        public static string IntToCurrency(int price, string Currency)
        {
            return IntToNumeric(price) + " " + Currency;
        }

        public static int CurrencyToInt(string Currency, string mark)
        {
            return Convert.ToInt32(Currency
                                    .Replace(",", "")
                                    .Replace(".", "")
                                    .Replace(mark, "")
                                    .Replace(" ", ""));
        }

        public static string ToVND(int price)
        {
            if (price == 0) return "0";

            CultureInfo elGRs = CultureInfo.CreateSpecificCulture("en-US");
            return String.Format(elGRs, "{0:0,0}", Convert.ToDouble(price));
        }
    }
}
