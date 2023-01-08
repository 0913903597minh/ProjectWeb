using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWeb.Models
{
    public class CartModel
    {
        public string phone { get; set; }

        public string name { get; set; }
        public string address { get; set; }

        public string note { get; set; }

        public List <ProductModel> list_order { get; set; } = new List<ProductModel>();
    }
}