using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Configuration
{
    public static class ViewConfig
    {
        public static string ALL = "All";
        public static string TOPSALE = "TOPSALE";
        public static string PRODUCTNEW = "PRODUCTNEW";
        public static string DISCOUNT = "DISCOUNT";
        public static string MALE = "MALE";
        public static string FEMALE = "FEMALE";
        public static string BOY = "BOY";
        public static string GIRL = "GIRL";
        public static int MALE_INT = 1;
        public static int FEMALE_INT = 2;
        public static int BOY_INT = 3;
        public static int GIRL_INT = 4;

        public static Dictionary<string, string> StatusToDictionaryProduct = new Dictionary<string, string>()
        {
            {ALL, "Tất cả" },
            {TOPSALE, "Hot" },
            {PRODUCTNEW, "Sản phẩm mới" },
            {DISCOUNT, "Sản phẩm khuyễn mãi" },
        };

        public static Dictionary<string, string> StatusToDictionarySort = new Dictionary<string, string>()
        {
              {ALL, "Sắp xếp" },
            {TOPSALE, "Hot" },
            {PRODUCTNEW, "Sản phẩm mới" },
            {DISCOUNT, "Sản phẩm khuyễn mãi" },
        };

        public static int POSTPAID_INT = 1;// trả sau
        public static int PREPAY_INT = 2; // trả trước
        public static int BUYINSTORE_INT = 3; // mua tại cửa hàng

        public static Dictionary<int?, string> StatusToDictionaryPay = new Dictionary<int?, string>()
        {
            {-1, "Tất cả hình thức thanh toán" },
            {POSTPAID_INT, "Thanh toán khi nhận hàng" },
            {PREPAY_INT, "Thanh toán chuyển khoản" },
            {BUYINSTORE_INT, "Mua và thanh toán tại cửa hàng" },
        };

        public static List<int> ListPay = new List<int>()
        {
            POSTPAID_INT,PREPAY_INT,BUYINSTORE_INT
        };
    }

}
