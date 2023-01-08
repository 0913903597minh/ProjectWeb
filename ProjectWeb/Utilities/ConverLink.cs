using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Utilities
{
    public static class ConverLink
    {
        public static string CompareLink(string str)
        {
            str = ConvertVietChar.convertToUnSign2(str.ToLower()).Replace(" ", "-")
                .Replace(".","").Replace("?","")
                .Replace(",", "").Replace(":", "")
                .Replace(";", "").Replace("!", "");
            return str;
        }
    }
}
