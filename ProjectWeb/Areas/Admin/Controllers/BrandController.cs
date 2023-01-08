using PagedList;
using ProjectWeb.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectWeb;
using ProjectWeb.Service;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class BrandController : Controller
    {
        // GET: Brand
        public ActionResult Index(int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            return View();
        }
        public ActionResult ListBrand(int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            int skip = (page - 1) * size;
            BrandService brandService = new BrandService();
            var list = brandService.GetAll(skip, size);
            var count = brandService.CountAll();
            StaticPagedList<Brand> pagedList = new StaticPagedList<Brand>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddBrand(int? id)
        {
            Brand brand = new Brand();
            if (id.HasValue)
            {
                brand = new BrandService().GetById(id.Value);
            }
            return View(brand);
        }
        [HttpPost]
        public ActionResult AddBrand(Brand model)
        {
            BrandService brandService = new BrandService();
            if (string.IsNullOrEmpty(model.brand_name))
            {
                ModelState.AddModelError("brand_name", "Tên thương hiệu không được để trống");
            }
            else if (brandService.CheckName(model))
            {
                ModelState.AddModelError("brand_name", "Tên thương hiệu đã tồn tại");
            }
            if (ModelState.IsValid)
            {
                if (model.brand_id > 0)
                {
                    if (brandService.Update(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
                else
                {
                    if (brandService.Insert(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
            }
            return View(model);
        }

        public bool Delete(int id)
        {
            BrandService brandService = new BrandService();
            Brand brand = brandService.GetById(id);
            if (brand != null)
            {
                brand.is_delete = true;
                //brand.update_date = DateTime.Now;
                if (brandService.Update())
                    return true;
            }
            return false;
        }
    }
}