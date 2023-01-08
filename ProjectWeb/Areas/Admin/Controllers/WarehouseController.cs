using System.Web.Mvc;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ProjectWeb.Infrastructure;
using ProjectWeb;
using ProjectWeb.Service;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class WarehouseController : Controller
    {
        // GET: Warehouse
        public ActionResult Index(int page =1,int size =10)
        {
            return View();
        }
        public ActionResult ListWarehouse(int productImgid,int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            ViewBag.productImgid = productImgid;
            int skip = (page - 1) * size;
            WarehouseService warehouseService= new WarehouseService();
            var list = warehouseService.GetAllByKey(productImgid, skip, size);
            var count = warehouseService.CountAllBykey(productImgid);
            StaticPagedList<Warehouse> pagedList = new StaticPagedList<Warehouse>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddWarehouse(int? id,int productImgid)
        {
            ViewBag.ProductImg = new ProductImgService().GetById(productImgid);
            Warehouse warehouse = new Warehouse();
            warehouse.productimg_id = productImgid;
            warehouse.discount = 0;
            if (id.HasValue)
            {
                warehouse = new WarehouseService().GetById(id.Value);
            }
            return View(warehouse);
        }
        [HttpPost]
        public ActionResult AddWarehouse(Warehouse model)
        {
            ViewBag.ProductImg = new ProductImgService().GetById(model.productimg_id.Value);
            WarehouseService warehouseService = new WarehouseService();
            //if (string.IsNullOrEmpty(model.Name))
            //{
            //    ModelState.AddModelError("Name", "Tên hãng không được để trống");
            if (model.discount<0&& model.discount>99)
            {
                ModelState.AddModelError("Discount", "Giảm giá không khả dụng");
            }
            if (ModelState.IsValid)
            {

                if (model.id > 0)
                {
                    if (warehouseService.Update(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
                else
                {
                    model.status = 1;
                    model.amount = 0;
                    if (warehouseService.Insert(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
            }
            return View(model);
        }
    }
}