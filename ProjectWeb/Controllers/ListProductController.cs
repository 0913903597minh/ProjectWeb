using ProjectWeb.Configuration;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ProjectWeb.Service;
using Newtonsoft.Json;

namespace ProjectWeb.Controllers
{
    public class ListProductController : Controller
    {
        // GET: ListProduct
        public ActionResult Index(List<int> brand, int page = 1, int size = 6, string key = "", int sex = -1, string sort = "", int sizeP = -1)
        {
            ProjectWeb.Service.ViewService viewService = new ProjectWeb.Service.ViewService();
            ViewBag.page = page;
            ViewBag.size = size;
            ViewBag.sizeP = sizeP;
            ViewBag.key = key;
            ViewBag.sex = sex;
            ViewBag.brand = brand;
            ViewBag.sort = sort;
            ViewBag.ListBrand = new ProjectWeb.Service.BrandService().GetAll();

            return View();
        }

        public ActionResult SearchList(List<int> brand, int page = 1, int size = 6, string key = "", int sex = -1, string sort = "", int sizeP = -1)
        {
            ProjectWeb.Service.ViewService viewService = new ProjectWeb.Service.ViewService();
            ViewBag.page = page;
            ViewBag.size = size;
            ViewBag.sizeP = sizeP;
            ViewBag.key = key;
            ViewBag.sex = sex;
            ViewBag.brand = brand;
            ViewBag.sort = sort;
            ViewBag.ListBrand = new ProjectWeb.Service.BrandService().GetAll();
            if (string.IsNullOrEmpty(sort))
            {
                sort = ViewConfig.ALL;
            }
            int skip = (page - 1) * size;
            List<ProjectWeb.Product> list = viewService.GetAllByAllKey(key, brand, sex, sort, sizeP, skip, size);
            int count = viewService.CountAllByAllKey(key, brand, sex, sort, sizeP);
            ViewBag.count = count;
            StaticPagedList<ProjectWeb.Product> pagedlist = new StaticPagedList<ProjectWeb.Product>(list, page, size, count);
            return View(pagedlist);
        }
        public ActionResult DetailProduct(int id)
        {
            ProjectWeb.Product product = new ProjectWeb.Service.ProductService().GetById(id);
            return View(product);
        }
        public ActionResult SelectSize(int id)
        {
            List<ProjectWeb.Warehouse> list = new ProjectWeb.Service.WarehouseService().GetAllByKey(id);
            return View(list);
        }
        public ActionResult SelectSizeSearch(int sex, int selected = -1)
        {
            ViewBag.selected = selected;
            List<ProjectWeb.Size> list = new ProjectWeb.Service.SizeService().GetAllBySex(sex);
            return View(list);
        }
        public ActionResult ListAllProductSimilar(int id, int brandid, int type)
        {
            ViewBag.brandid = brandid;
            ViewBag.type = type;
            List<ProjectWeb.Product> list = new List<ProjectWeb.Product>();
            ProjectWeb.Service.ViewService viewService = new ProjectWeb.Service.ViewService();

            list = viewService.GetAllProductSimilar(id, brandid, type, 0, 4);

            return View(list);
        }

        public bool OrderProduct(int id, int amount)
        {
            // string key;
            //ProjectWeb.Service.CartProvider cartProvider = new ProjectWeb.Service.CartProvider();
            //ProjectWeb.Service.CookieProvider cookieProvider = new ProjectWeb.Service.CookieProvider();
            //ProjectWeb.Cookie cookie = new ProjectWeb.Cookie();
            //double timeout = Convert.ToDouble(new ProjectWeb.Service.ConfigProvider().GetTimeOut_Hours_Cookie());
            HttpCookie httpCookie = HttpContext.Request.Cookies["order"];
            List<ProjectWeb.Models.CookieModel> list = new List<Models.CookieModel>();
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie("order");
                //httpCookie["keycode"] = Guid.NewGuid().ToString();
                //httpCookie.Expires = DateTime.Now.AddHours(timeout);
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<ProjectWeb.Models.CookieModel>>(httpCookie.Value) ?? new List<Models.CookieModel>();
            }
            var item = list.FirstOrDefault(x => x.id == id);
            if (item!=null)
            {
                item.amount += amount;
            }
            else
            {
                list.Add(new Models.CookieModel { id = id, amount = amount });
            }
            httpCookie.Value = JsonConvert.SerializeObject(list);
            HttpContext.Response.Cookies.Add(httpCookie);
            //key = httpCookie["keycode"];
            //cookie = cookieProvider.GetByKey(key);
            //if (cookie == null)
            //{
            //    httpCookie = new HttpCookie("key");
            //    httpCookie["keycode"] = Guid.NewGuid().ToString();
            //    httpCookie.Expires = DateTime.Now.AddHours(timeout);
            //    HttpContext.Response.Cookies.Add(httpCookie);
            //    cookie = new ProjectWeb.Cookie()
            //    {
            //        KeyCode = httpCookie["keycode"],
            //        ExpiredDate = DateTime.Now.AddHours(timeout),
            //    };
            //    ProjectWeb.Cart newCart = new ProjectWeb.Cart()
            //    {
            //        WarehouseId = id,
            //        Amount = 1,
            //        CookieId = cookie.Id,
            //        status = CartConfig.INCART,
            //        create_date = DateTime.Now,
            //    };
            //    cookie.Carts.Add(newCart);
            //    if (cookieProvider.Insert(cookie))
            //        return true;
            //    return false;
            //}
            //else
            //{
            //    httpCookie.Expires = DateTime.Now.AddHours(timeout);
            //    HttpContext.Response.Cookies.Add(httpCookie);
            //    cookie.ExpiredDate = DateTime.Now.AddHours(timeout);
            //    ProjectWeb.Cart cart = cookie.Carts.FirstOrDefault(x => x.WarehouseId == id && x.Status == 1);
            //    if (cart == null)
            //    {
            //        cart = new ProjectWeb.Cart();
            //        cart.create_date = DateTime.Now;
            //        cart.status = CartConfig.INCART;
            //        cart.amount = 1;
            //        cart.CookieId = cookie.Id;
            //        cart.warehouse_id = id;
            //        cookie.Carts.Add(cart);
            //    }
            //    //else
            //    //{
            //    //    cart.Amount += 1;
            //    //    cart.UpdateDate = DateTime.Now;
            //    //}
            //    if (cookieProvider.Update(cookie))
            //    {
            //        Response.StatusCode = (int)HttpStatusCode.Created;
            //        return true;
            //    }
            //    return false;
            //}
            Response.StatusCode = (int)HttpStatusCode.Created;
            return true;
        }

    }
}