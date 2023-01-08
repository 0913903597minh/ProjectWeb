using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWeb.Areas.Admin.Models

{
    public class DashboardModel
    {
        public List<string> lable { set; get; }
        public List<dynamic> result { set; get; }
        public List<string> color { set; get; }
    }
}