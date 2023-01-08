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
    public class ImportUnitController : Controller
    {
        // GET: ImportUnit
        public ActionResult Index(int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            return View();
        }
        public ActionResult ListImportUnit(int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            int skip = (page - 1) * size;
            ImportUnitService importUnitProvider = new ImportUnitService();
            var list = importUnitProvider.GetAll(skip, size);
            var count = importUnitProvider.CountAll();
            StaticPagedList<ImportUnit> pagedList = new StaticPagedList<ImportUnit>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddImportUnit(int? id)
        {
            ImportUnit importUnit = new ImportUnit();
            if (id.HasValue)
            {
                importUnit = new ImportUnitService().GetById(id.Value);
            }
            return View(importUnit);
        }
        [HttpPost]
        public ActionResult AddImportUnit(ImportUnit model)
        {
            ImportUnitService importUnitProvider = new ImportUnitService();
            if (string.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("Name", "Tên nhà cung cấp không được để trống");
            }
            else if(importUnitProvider.CheckName(model))
            {
                ModelState.AddModelError("Name", "Tên nhà cung cấp đã tồn tại");
            }
            if (string.IsNullOrEmpty(model.address))
            {
                ModelState.AddModelError("Address", "Địa chỉ nhà cung cấp không được để trống");
            }
            if (string.IsNullOrEmpty(model.phone))
            {
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
            }
            else if (model.phone.Length != 10 || model.phone[0] != '0')
            {
                ModelState.AddModelError("Phone", "Số điện thoại không phù hợp");
            }
            if (ModelState.IsValid)
            {

                if (model.id > 0)
                {
                    if (importUnitProvider.Update(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);

                    }
                    ModelState.AddModelError("Error", "Lỗi hệ thống");
                }
                else
                {
                    if (importUnitProvider.Insert(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                    ModelState.AddModelError("Error", "Lỗi hệ thống");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ImportUnitDetail(int id)
        {
               ImportUnit importUnit = new ImportUnitService().GetById(id);
            return View(importUnit);
        }
    }
}