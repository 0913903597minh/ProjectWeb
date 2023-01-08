using PagedList;
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
    public class ImportController : Controller
    {
        // GET: Import
        // GET: ImportUnit
        public ActionResult Index(int id,int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            ViewBag.id = id;
            return View(new ImportUnitService().GetById(id));
        }
        public ActionResult ListImport(int id, int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            ViewBag.id = id;
            int skip = (page - 1) * size;
            ImportService importService = new ImportService();
            var list = importService.GetAllByKey(id, skip, size);
            var count = importService.CountAllByKey(id);
            StaticPagedList<Import> pagedList = new StaticPagedList<Import>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddImport(int ?id, int? importunitid)
        {
         
            Import import = new Import();
            import.importunit_id = importunitid??0;
            import.ImportDetails = new List<ImportDetail>();
            import.price = 0;
            if (id.HasValue)
            {
                import = new ImportService().GetById(id.Value);
            }
         
            ViewBag.ImportUnit = new ImportUnitService().GetAll();
            return View(import);
        }
        [HttpPost]
        public ActionResult AddImport(Import model)
        {
            double total = 0;
            ImportUnitService  importUnitService= new ImportUnitService();
            ImportService importService = new ImportService();
            WarehouseService warehouseService = new WarehouseService();
            ViewBag.ImportUnit = new ImportUnitService().GetAll();
            if (model.importunit_id.HasValue)
            {
                if (!importUnitService.GetAll().Select(x => x.id).Contains(model.importunit_id.Value))
                {
                    ModelState.AddModelError("ImportUnitId", "Nhà cung cấp không tồn tại.");
                }
            }
            else
            {
                ModelState.AddModelError("ImportUnitId", "Vui lòng chọn nhà cung cấp.");
            }
            if (model.ImportDetails == null || model.ImportDetails.Count<1)
            {
                ModelState.AddModelError("error", "Vui lòng thêm sản phẩm vào phiếu nhập.");
            }
            else
            {
               var check = model.ImportDetails.GroupBy(x => x.warehouse_id.Value).ToList();
                if (model.ImportDetails.Count != check.Count)
                {
                    ModelState.AddModelError("error", "Lỗi.");
                }
                else
                {
                    foreach (var item in model.ImportDetails)
                    {
                        Warehouse warehouse = warehouseService.GetById(item.warehouse_id.Value);
                        if (warehouse != null)
                        {
                            if(item.price!=null&&item.price>10000&& item.amount != null && item.amount > 0)
                            {
                                if (string.IsNullOrEmpty(warehouse.code))
                                {
                                    warehouse.code = warehouse.ProductImg.product_id.Value.ToString() + warehouse.productimg_id.Value.ToString() + warehouse.size_id.Value.ToString();
                                }
                                warehouse.amount += item.amount;
                                warehouse.update_date = DateTime.Now;
                                total += item.price.Value;
                            }
                            else
                            {
                                ModelState.AddModelError("error", "Lỗi.");
                                break;
                            }
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                foreach (var item in model.ImportDetails)
                {
                    item.create_date = DateTime.Now;
                    item.status = 1;
                }
                model.create_date = DateTime.Now;
                model.status = 1;
                model.price = total;
                if (importService.Insert(model))
                {
                    warehouseService.Update();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(importService.GetById(model.id));
                }
            }
            return View(model);
        }
       
        [HttpPost]
        public ActionResult UpdateImport(Import model)
        {
            double total = 0;
            ImportUnitService importUnitService = new ImportUnitService();
            ImportService importService = new ImportService();
            WarehouseService warehouseService = new WarehouseService();
            ViewBag.ImportUnit = new ImportUnitService().GetAll();
            if (model.importunit_id.HasValue)
            {
                if (!importUnitService.GetAll().Select(x => x.id).Contains(model.importunit_id.Value))
                {
                    ModelState.AddModelError("ImportUnitId", "Nhà cung cấp không tồn tại.");
                }
            }
            else
            {
                ModelState.AddModelError("ImportUnitId", "Vui lòng chọn nhà cung cấp.");
            }
            if (model.ImportDetails == null)
            {
                ModelState.AddModelError("error", "Vui lòng thêm sản phẩm vào phiếu nhập.");
            }
            else
            {
                var check = model.ImportDetails.GroupBy(x => x.warehouse_id.Value).ToList();
                if (model.ImportDetails.Count != check.Count)
                {
                    ModelState.AddModelError("error", "Lỗi.");
                }
                else
                {
                    foreach (var item in model.ImportDetails)
                    {
                        Warehouse warehouse = warehouseService.GetById(item.warehouse_id.Value);
                        if (warehouse != null)
                        {
                            if (item.price != null && item.price >= 10000 && item.amount != null && item.amount >= 1)
                            {
                                total += item.price.Value;
                            }
                            else
                            {
                                ModelState.AddModelError("error", "Lỗi.");
                                break;
                            }
                        }
                    }
                }
            }
            model.price = total;
            return View("AddImport", model);
        }
    }
}