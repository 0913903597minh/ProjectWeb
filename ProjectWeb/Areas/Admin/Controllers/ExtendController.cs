using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Infrastructure;
using ProjectWeb.Service;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class ExtendController : Controller
    {
        // GET: Extend
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult SelectSize(int? productimgid, int? selecteds,string list)
        {
            if (string.IsNullOrEmpty(list))
                list = "0";
            List<string> listDatas = list.Split(',').ToList();
            ViewBag.listDatas = listDatas;
            int type = new ProductImgService().GetById(productimgid.Value).Product.type.Value;
            ViewBag.Type = type;
            ViewBag.selecteds = selecteds??-1;
            return View(new SizeService().GetAllBySex(type));
        }
        public ActionResult FroalaUploadImage(HttpPostedFileBase file, int? postId)
        {
            var fileName = Path.GetFileName(file.FileName);
            var rootPath = Server.MapPath("~/Images/Upload/");
            file.SaveAs(Path.Combine(rootPath, fileName));
            return Json(new { link = "/Images/Upload/" + fileName }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintOrder(int orderid)
        {
            return View( new OrderService().GetById(orderid));
        }
        //public int CoutOrder(int status)
        //{
        //    return new OrderService().CountOrderByStatus(status);
        //}
    }
}