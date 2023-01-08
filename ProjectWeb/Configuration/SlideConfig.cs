using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Configuration
{
    public class SlideConfig
    {
        public enum Status
        {
            SHOW = 1,
            HIDDEN = 0,
        }
        public static Dictionary<int, string> StatusToDictionaryStatusHTML = new Dictionary<int, string>()
        {
            {-1, "Tất cả" },
            {(int)Status.SHOW, "Hiển thị" },
            {(int)Status.HIDDEN, "Ẩn" },
        };
        public static Dictionary<int, string> StatusToDictionaryStatus = new Dictionary<int, string>()
        {
            {(int)Status.SHOW, "Hiển thị" },
            {(int)Status.HIDDEN, "Ẩn" },
        };
    }
}
