using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Configuration
{
    public class ProductConfig
    {
        public enum Type
        {
          MALE = 1,
          FEMALE = 2,
          BOY = 3,
          GIRL = 4,
        }
        public enum Status
        {
            ACTIVE = 1,
            STOPSELL = 2,
            ISDELETE = 1,
        }
        public static Dictionary<int, string> StatusToDictionaryHTML = new Dictionary<int, string>()
        {
            {-1, "Tất cả trạng thái" },
           // {(int)Status.WAITFORPAY, "Chờ thanh toán" },
            {(int)Status.ACTIVE, "Hoạt động" },
            {(int)Status.STOPSELL, "Dừng bán" },
        };
        public static Dictionary<int, string> StatusToDictionarySelect = new Dictionary<int, string>()
        {
          //  {-1, "Tất cả trạng thái đơn hàng" },
           // {(int)Status.WAITFORPAY, "Chờ thanh toán" },
            {(int)Status.ACTIVE, "Hoạt động" },
            {(int)Status.STOPSELL, "Dừng bán" },
        };
    }
}
