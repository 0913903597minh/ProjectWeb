using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Infrastructure;
using ProjectWeb.Service;
using ProjectWeb;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class ImportDetailController : Controller
    {
        // GET: Admin/ImportDetail
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddImportDetail()
        {
            ViewBag.ProductImg = new ProductImgService().GetAll();

            return View(new ImportDetail() {amount = 0, price = 0, Warehouse = new Warehouse() }); ;
        }
        [HttpPost]
        public ActionResult AddImportDetail(ImportDetail model,string list)
        {
            if (string.IsNullOrEmpty(list))
                list = "0";
            List<string> listDatas = list.Split(',').ToList();
            ViewBag.ProductImg = new ProductImgService().GetAll();
            WarehouseService warehouseService = new WarehouseService();
            if(!(new ProductImgService().GetAll().Select(x=>x.productimg_id).Contains(model.Warehouse.productimg_id.Value)) || !(new SizeService().GetAll().Select(x => x.size_id).Contains(model.Warehouse.size_id.Value)))
            {
                ModelState.AddModelError("error","Lỗi hệ thống");
            }
            if(!warehouseService.CheckSize(model.Warehouse))
            {
                
                warehouseService.Insert(model.Warehouse);
            }
            else
            {
                model.Warehouse = warehouseService.GetBySize(model.Warehouse.productimg_id.Value, model.Warehouse.size_id.Value);
            }
            if (model.amount < 1)
            {
                ModelState.AddModelError("Amount", "Số lượng không khả dụng.");
            }
            if (model.price < 10000)
            {
                ModelState.AddModelError("Price", "Giá không khả dụng.");
            }
            if (ModelState.IsValid)
            {
                if (list.Contains(model.Warehouse.size_id.ToString()))
                {
                    ModelState.AddModelError("error", "Sản phẩm đã có trong phiếu nhập.");
                    return View(model);
                }
                model.warehouse_id = model.Warehouse.id;
                Response.StatusCode = (int)HttpStatusCode.Created;
                return PartialView("trAddImportDetail", model);
            }
            return View(model);
        }
    }
}