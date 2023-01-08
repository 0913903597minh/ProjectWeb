using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Infrastructure;
using ProjectWeb.Configuration;
using ProjectWeb;
using ProjectWeb.Service;


namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DashboardOrder()
        {
            OrderService orderService = new OrderService();
            Models.DashboardOrderModel dashboardOrderModel = new Models.DashboardOrderModel();
            dashboardOrderModel.CountWait = orderService.CountAllByStatus( (int)OrderConfig.Status.WAIT);
            dashboardOrderModel.CountCancel = orderService.CountAllByStatus((int)OrderConfig.Status.CANCEL);
            dashboardOrderModel.CountConfirm = orderService.CountAllByStatus( (int)OrderConfig.Status.CONFIRM);
            dashboardOrderModel.CountFinish = orderService.CountAllByStatus((int)OrderConfig.Status.FINISH);
            return View(dashboardOrderModel);
        }
        public ActionResult DashboardRevenue()
        {
            DashboardService dashboardService = new DashboardService();
            List<string> lable = new List<string>();
            List<dynamic> result = new List<dynamic>();
            List<string> color = new List<string>();
            DateTime dt = DateTime.Now;
            //DateTime ss = CultureInfo.CurrentCulture.GetFirstDayOfWeek
            Models.DashboardModel dashboardModel = new Models.DashboardModel();
            lable.Add("Doanh thu ngày " + dt.ToString("dd-MM-yyyy"));
            result.Add(dashboardService.RevenueDay(dt));
            lable.Add("Doanh thu tháng " + dt.ToString("MM"));
            result.Add(dashboardService.RevenueMonth(dt));
            lable.Add("Doanh thu năm " + dt.ToString("yyyy"));
            result.Add(dashboardService.RevenueYear(dt));
            Random r = new Random();
            int k = r.Next(1, 10);
            foreach (var item in result)
            {  
                int p = r.Next(1, 10);
                k =(k+ p)%ExtendConfig.Color.Count;
               color.Add(ExtendConfig.Color[k]);
            }
            dashboardModel.lable = lable;
            dashboardModel.result = result;
            dashboardModel.color = color;
            return View(dashboardModel);
        }
        public ActionResult DashboardProductSold()
        {
            DashboardService dashboardService = new DashboardService();
            List<string> lable = new List<string>();
            List<dynamic> result = new List<dynamic>();
            List<string> color = new List<string>();
            DateTime dt = DateTime.Now;
            Models.DashboardModel dashboardModel = new Models.DashboardModel();
            lable.Add("Top 3 sản phẩm dự đoán bán chạy");
            result.Add(dashboardService.TopBestSale());
            lable.Add("Top 3 sản phẩm dự đoán bán chậm");
            result.Add(dashboardService.TopBadSale());
            Random r = new Random();
            int k = r.Next(1, 10);
            foreach (var item in result)
            {
                int p = r.Next(1, 10);
                k = (k + p) % ExtendConfig.Color.Count;
                color.Add(ExtendConfig.Color[k]);
            }
            dashboardModel.lable = lable;
            dashboardModel.result = result;
            dashboardModel.color = color;
            return View(dashboardModel);
        }
        public ActionResult DashboardProductSize()
        {
            DashboardService dashboardService = new DashboardService();
            List<string> lable = new List<string>();
            List<dynamic> result = new List<dynamic>();
            List<string> color = new List<string>();
            DateTime dt = DateTime.Now;
            Models.DashboardModel dashboardModel = new Models.DashboardModel();
            lable.Add("Top 3 size dự đoán bán chạy");
            result.Add(dashboardService.TopBestSize());
            lable.Add("Top 3 size dự đoán bán chậm");
            result.Add(dashboardService.TopBadSize());
            Random r = new Random();
            int k = r.Next(1, 10);
            foreach (var item in result)
            {
                int p = r.Next(1, 10);
                k = (k + p) % ExtendConfig.Color.Count;
                color.Add(ExtendConfig.Color[k]);
            }
            dashboardModel.lable = lable;
            dashboardModel.result = result;
            dashboardModel.color = color;
            return View(dashboardModel);
        }
        public ActionResult DashboardInventory()
        {
            DashboardService dashboardService = new DashboardService();
            List<string> lable = new List<string>();
            List<dynamic> result = new List<dynamic>();
            List<string> color = new List<string>();
            DateTime dt = DateTime.Now;
            Models.DashboardModel dashboardModel = new Models.DashboardModel();
            lable.Add("Sản phẩm còn nhiều");
            result.Add(dashboardService.Stocking());
            lable.Add("Sản phẩm sắp hết");
            result.Add(dashboardService.OutOfStock());
            lable.Add("Sản phẩm chưa nhập hàng");
            result.Add(dashboardService.NotImport());
            Random r = new Random();
            int k = r.Next(1, 10);
            foreach (var item in result)
            {
                int p = r.Next(1, 10);
                k = (k + p) % ExtendConfig.Color.Count;
                color.Add(ExtendConfig.Color[k]);
            }
            dashboardModel.lable = lable;
            dashboardModel.result = result;
            dashboardModel.color = color;
            return View(dashboardModel);
        }
    }
}