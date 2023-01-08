using ProjectWeb.Service;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.Services.Description;
using ProjectWeb.Configuration;
using System.Drawing;
using System.Web.UI;

namespace ProjectWeb.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            SlideService slideService = new SlideService();
            var list = slideService.GetAllByStatus(1);
            return View(list);
        }
        public ActionResult ListProduct(string type)
        {
            ViewBag.Type = type;
            if (type == ViewConfig.TOPSALE)
            {
                var list = new ViewService().GetAllTopSale(0, 4);
                if (list.Count() > 0) {
                    return View(list);
                }
                else {
                    return View(new ViewService().GetAllProductNew(0, 4));
                }
            }
            if (type == ViewConfig.PRODUCTNEW)
            {
                return View(new ViewService().GetAllProductNew(0, 4));
            }
            //if (type == ViewConfig.DISCOUNT)
            //{
            //    var list = new ViewService().GetAllProductDiscount(0, 4);
            //    return View(list);
            //}
            if (type == ViewConfig.MALE)
            {
                return View(new ViewService().GetAllProductMale(0, 4));
            }
            if (type == ViewConfig.FEMALE)
            {
                return View(new ViewService().GetAllProductFemale(0, 4));
            }
            if (type == ViewConfig.BOY)
            {
                return View(new ViewService().GetAllProductBoy(0, 4));
            }
            if (type == ViewConfig.GIRL)
            {
                return View(new ViewService().GetAllProductGirl(0, 4));
            }
            return View();
        }
    }
}