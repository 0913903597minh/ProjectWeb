using PagedList;
using ProjectWeb.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class ProductController : Controller
    {
        public const string _ImagesPath = "~/Images/Products";
        // GET: Product
        public ActionResult Index(string keysearch, int sex = -1, int brandid = -1, int status = -1, int page = 1, int size = 10)
        {
            ViewBag.Brand = new BrandService().GetAll();
            ViewBag.keysearch = keysearch;
            ViewBag.page = page;
            ViewBag.size = size;
            ViewBag.sex = sex;
            ViewBag.status = status;
            ViewBag.brandid = brandid;
            return View();
        }
        public ActionResult ListProduct(string keysearch, int sex = -1, int brandid = -1, int status = -1, int page = 1, int size = 10)
        {
            ViewBag.keysearch = keysearch;
            ViewBag.page = page;
            ViewBag.size = size;
            ViewBag.brandid = brandid;
            ViewBag.sex = sex;
            ViewBag.status = status;
            int skip = (page - 1) * size;
            ProductService productService = new ProductService();
            var list = productService.GetAllByKey(keysearch, brandid, sex, status, skip, size);
            var count = productService.CountAllByKey(keysearch, brandid, sex, status);
            StaticPagedList<Product> pagedList = new StaticPagedList<Product>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddProduct(int? id)
        {
            ViewBag.Brand = new BrandService().GetAll();
            Product product = new Product();
            product.discount = 0;
            product.product_price = 0;
            if (id.HasValue)
            {
                product = new ProductService().GetById(id.Value);
            }
            return View(product);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddProduct(Product model, HttpPostedFileBase avatar_url)
        {
            model.discount = model.discount ?? 0;
            ViewBag.Brand = new BrandService().GetAll();
            ProductService productService = new ProductService();

            if (string.IsNullOrEmpty(model.product_name))
            {
                ModelState.AddModelError("product_name", "Tên giày không được để trống");
            }
            else if (productService.CheckNameAndType(model))
            {
                ModelState.AddModelError("product_name", "Tên giày đã tồn tại");
            }
            if (model.product_price < 50000)
            {
                ModelState.AddModelError("product_price", "Giá không khả dụng");
            }
            if (model.product_price == null)
            {
                ModelState.AddModelError("product_price", "Giá không được để trống");
            }
            if (model.discount < 0 || model.discount > 99)
            {
                ModelState.AddModelError("discount", "Khuyễn mãi không khả dụng");
            }
            if (model.product_id == 0 && avatar_url == null)
            {
                ModelState.AddModelError("avatar_url", "Thêm ảnh giày");
            }
            if (avatar_url != null)
            {
                try
                {
                    string fileName = Guid.NewGuid() + Path.GetFileName(avatar_url.FileName);
                    string path = Path.Combine(Server.MapPath(_ImagesPath), fileName);
                    avatar_url.SaveAs(path);
                    model.avatar_url = fileName;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("avatar_url", "Thêm ảnh giày bị lỗi");
                }
            }
            if (ModelState.IsValid)
            {
                if (model.brand_id < 0)
                    model.brand_id = null;
                if (model.product_id > 0)
                {
                    if (avatar_url == null)
                    {
                        var old = productService.GetById(model.product_id);
                        model.avatar_url = old.avatar_url;
                    }
                    if (productService.Update(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
                else
                {
                    if (productService.Insert(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View(model);
                    }
                }
            }
            return View(model);
        }
        public void UpdateCode()
        {
            new WarehouseService().UpdateCode();
        }
        [HttpGet]
        public ActionResult PromotionProduct()
        {
            ViewBag.discount = 0;
            ViewBag.brandid = new List<int>();
            ViewBag.productid = new List<int>();
            ViewBag.brand = new BrandService().GetAll();
            ViewBag.product = new ProductService().GetAll();
            return View();
        }
        [HttpPost]
        public ActionResult PromotionProduct(List<int> productid, List<int> brandid, int? discount)
        {
            discount = discount ?? 0;
            if (discount > 100 || discount < 0)
            {
                ModelState.AddModelError("discount", "Khuyễn mãi không khả dụng");
            }
            brandid = brandid ?? new List<int>();
            productid = productid ?? new List<int>();
            ViewBag.brand = new BrandService().GetAll();
            ViewBag.product = new ProductService().GetAll();
            ViewBag.brandid = brandid;
            ViewBag.discount = discount;
            ViewBag.productid = productid;
            ProductService productService = new ProductService();
            if (ModelState.IsValid)
            {
                if (productService.DiscountProduct(productid, brandid, discount.Value))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View();
                }
                ModelState.AddModelError("error", "Lỗi");
                return View();
            }
            return View();
        }
        public bool Delete(int id)
        {
            ProductService productService = new ProductService();
            Product product = productService.GetById(id);
            if (product != null)
            {
                product.is_delete = true;
                product.update_date = DateTime.Now;
                if (productService.Update())
                    return true;
            }
            return false;
        }
    }
}