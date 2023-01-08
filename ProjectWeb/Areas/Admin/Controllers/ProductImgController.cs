using PagedList;
using ProjectWeb.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Service;
using ProjectWeb;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class ProductImgController : Controller
    {
        public const string _ImagesPath = "~/Images/Products";
        // GET: ProductImg
        public ActionResult Index(int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            return View();
        }
        public ActionResult ListProductImg(int productid, int page = 1, int size = 5)
        {
            ViewBag.productid = productid;
            ViewBag.page = page;
            ViewBag.size = size;
            int skip = (page - 1) * size;
            ProductImgService productImg = new ProductImgService();
            var list = productImg.GetAllByProductId(productid, skip, size);
            var count = productImg.CountAllByProductId(productid);
            StaticPagedList<ProductImg> pagedList = new StaticPagedList<ProductImg>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddProductImg(int? id, int productid)
        {
            ProductImg productImg = new ProductImg();
            productImg.product_id = productid;
            if (id.HasValue)
            {
                productImg = new ProductImgService().GetById(id.Value);
            }
            return View(productImg);
        }
        [HttpPost]
        public ActionResult AddProductImg(ProductImg model, HttpPostedFileBase Url)
        {
            ProductImgService productImgService = new ProductImgService();
            if (model.productimg_id == 0 && Url == null)
            {
                ModelState.AddModelError("Url", "Thêm ảnh giày");
            }
            if (string.IsNullOrEmpty(model.color))
            {
                ModelState.AddModelError("Color", "Nhập màu");
            }
            else if (productImgService.CheckColor(model))
            {
                ModelState.AddModelError("Color", "Màu đã tồn tại");
            }
            if (Url != null)
            {
                try
                {
                    string fileName = Guid.NewGuid() + Path.GetFileName(Url.FileName);
                    string path = Path.Combine(Server.MapPath(_ImagesPath), fileName);
                    Url.SaveAs(path);
                    model.url = fileName;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Url", "Ảnh bị lỗi");
                }
            }
            if (ModelState.IsValid)
            {

                if (model.productimg_id > 0)
                {
                    if (Url == null)
                    {
                        var old = productImgService.GetById(model.productimg_id);
                        model.url = old.url;
                    }
                    if (productImgService.Update(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return PartialView("AddProductImg", model);
                    }
                }
                else
                {
                    if (productImgService.Insert(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return PartialView("AddProductImg", model);
                    }
                }
            }
            return PartialView("AddProductImg", model);
        }
        public bool Delete(int id)
        {
            ProductImgService productImgService = new ProductImgService();
            ProductImg productImg = productImgService.GetById(id);
            if (productImg != null)
            {
                productImg.is_delete = true;
                productImg.update_date = DateTime.Now;
                if (productImgService.Update())
                    return true;
            }

            return false;
        }
    }
}