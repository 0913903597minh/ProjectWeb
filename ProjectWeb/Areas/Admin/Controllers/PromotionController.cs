using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Infrastructure;
using ProjectWeb.Service;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class PromotionController : Controller
    {
        // GET: Promotion
        public ActionResult Index(int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            return View();
        }
        public ActionResult ListPromotion(int page = 1, int size = 10)
        {
            ViewBag.page = page;
            ViewBag.size = size;
            int skip = (page - 1) * size;
            PromotionService promotionService = new PromotionService();
            var list = promotionService.GetAll(skip, size);
            var count = promotionService.CountAll();
            StaticPagedList<Promotion> pagedList = new StaticPagedList<Promotion>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddPromotion(int? id)
        {
            Promotion promotion = new Promotion();
            promotion.prom_discount = 0;
            if (id.HasValue)
            {
                promotion = new PromotionService().GetById(id.Value);
            }

            return View(promotion);
        }
        [HttpPost]
        public ActionResult AddPromotion(Promotion model, string StartDate, string ExpiredDate)
        {
            DateTime d;
            if (!string.IsNullOrEmpty(StartDate))
            {
                if (DateTime.TryParseExact(StartDate, "dd-MM-yyyy",
                               null, DateTimeStyles.None,
                               out d))
                {
                    ModelState.Remove("StartDate");
                    model.prom_startdate = d;
                }
                else
                {
                    ModelState.Remove("StartDate");
                    model.prom_startdate = null;
                    ModelState.AddModelError("StartDate", "Giá trị không phù hợp");
                }
            }
            if (!string.IsNullOrEmpty(ExpiredDate))
            {
                model.prom_discount = model.prom_discount ?? 0;
                if (DateTime.TryParseExact(ExpiredDate, "dd-MM-yyyy",
                            null, DateTimeStyles.None,
                            out d))
                {
                    ModelState.Remove("ExpiredDate");
                    model.prom_expirationdate = d.AddDays(1);
                }
                else
                {
                    ModelState.Remove("ExpiredDate");
                    model.prom_expirationdate = null;
                    ModelState.AddModelError("ExpiredDate", "Giá trị không phù hợp");
                }
            }
            if (model.prom_startdate.HasValue && model.prom_expirationdate.HasValue)
            {
                if (model.prom_startdate > model.prom_expirationdate)
                    ModelState.AddModelError("error", "Thời gian kết thúc phải xáy ra sau thời gian bắt đầu!");
            }
            PromotionService promotionService = new PromotionService();
            if (string.IsNullOrEmpty(model.prom_keycode))
            {
                ModelState.AddModelError("prom_keycode", "Mã khuyễn mãi không được để trống");
            }
            else if (promotionService.CheckKeyCode(model))
            {
                ModelState.AddModelError("prom_keycode", "Mã khuyễn mãi đã tồn tại");
            }
            if (model.prom_discount.HasValue)
            {
                if (model.prom_discount < 0 || model.prom_discount > 99)
                {
                    ModelState.AddModelError("prom_discount", "Giảm giá không khả dụng");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.prom_id > 0)
                {
                    Promotion promotion = promotionService.GetById(model.prom_id);
                    promotion.prom_keycode= model.prom_keycode;
                    promotion.prom_startdate = model.prom_startdate;
                    promotion.prom_expirationdate = model.prom_expirationdate;
                    promotion.prom_amount = model.prom_amount;
                    promotion.prom_discount = model.prom_discount;
                    if (promotionService.Update())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
                else
                {
                    if (promotionService.Insert(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
            }
            return View(model);
        }
        public bool DeletePromotion(int id)
        {
            PromotionService promotionService = new PromotionService();
            Promotion promotion = promotionService.GetById(id);
            if (promotion != null)
            {
                promotion.is_delete = true;
                if (promotionService.Update())
                    return true;
            }
            return false;
        }
    }
}