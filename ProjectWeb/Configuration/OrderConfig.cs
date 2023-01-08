using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Configuration
{
    public class OrderConfig
    {
        public enum Status
        {
            WAIT = 1,
            CONFIRM = 2,
            CANCEL = 3,
            SHIP = 4,
            FINISH = 5,
            ISDELETE = 1,
            WAITFORPAY = 0,
        }
        public static Dictionary<int, string> StatusToDictionaryHTML = new Dictionary<int, string>()
        {
            {-1, "Tất cả trạng thái đơn hàng" },
           // {(int)Status.WAITFORPAY, "Chờ thanh toán" },
            {(int)Status.WAIT, "Chờ xử lý" },
            {(int)Status.CONFIRM, "Xác nhận" },
            {(int)Status.FINISH, "Hoàn thành" },
           {(int)Status.CANCEL, "Hủy" },
        };
        public static Dictionary<int, string> StatusToDictionaryALL = new Dictionary<int, string>()
        {
            {(int)Status.WAIT, "Chờ xử lý" },
            {(int)Status.CONFIRM, "Xác nhận" },
            {(int)Status.FINISH, "Hoàn thành" },
           {(int)Status.CANCEL, "Hủy" },
        };
        public static List<int> StatusToDictionaryALL_INT = new List<int>()
        {
            (int)Status.WAIT,
            (int)Status.CONFIRM,
            (int)Status.FINISH,
           (int)Status.CANCEL,
        };
        public static Dictionary<int, string> StatusToDictionaryWAIT = new Dictionary<int, string>()
        {
            {(int)Status.WAIT, "Chờ xử lý" },
            {(int)Status.CONFIRM, "Xác nhận" },
           {(int)Status.CANCEL, "Hủy" },
        };
        public static Dictionary<int, string> StatusToDictionaryCONFIRM = new Dictionary<int, string>()
        {
            {(int)Status.CONFIRM, "Xác nhận" },
            {(int)Status.FINISH, "Hoàn thành" },
            {(int)Status.CANCEL, "Hủy" },
        };
        public static Dictionary<int, string> StatusToDictionaryFINISH = new Dictionary<int, string>()
        {
            {(int)Status.FINISH, "Hoàn thành" },
            {(int)Status.CANCEL, "Hủy" },
        };
        public enum Pay
        {
            ALL = -1,
            PAID = 1,
            UNPAID = 0,
        }
        public static Dictionary<int, string> StatusToDictionaryPaidHTML = new Dictionary<int, string>()
        {   {(int)Pay.ALL, "Tất cả trạng thái thanh toán" },
            {(int)Pay.UNPAID, "Chưa thanh toán" },
            {(int)Pay.PAID, "Đã thanh toán" },
        };
    }
}
