using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWeb.Models
{
    public class ProductModel
    {
        public int warehouse_id { get; set; }
        public int warehouse_amount { get; set; }
        public int discount { get; set; }
        public string product_name { get; set; }
        public string product_color { get; set; }
        public double product_price { get; set; }

        public double size_VN { get; set; }
        public double size_US { get; set;}
        public double size_UK { get; set;}

    }
}