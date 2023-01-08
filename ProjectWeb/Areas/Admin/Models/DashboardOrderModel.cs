using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWeb.Areas.Admin.Models
{
    public class DashboardOrderModel
    {
        public int CountWait { set; get; }
        public int CountConfirm { set; get; }
        public int CountCancel { set; get; }
        public int CountFinish { set; get; }
    }
}