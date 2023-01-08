using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Configuration;
using PagedList;
using Newtonsoft.Json;

namespace ProjectWeb.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            HttpCookie httpCookie = HttpContext.Request.Cookies["order"];
            ProjectWeb.Service.WarehouseService warehouseService = new Service.WarehouseService();
            ProjectWeb.Models.CartModel cartModel = new Models.CartModel()
            {
                list_order = new List<Models.ProductModel>()
            };
            if (httpCookie != null)
            {
                var order = JsonConvert.DeserializeObject<List<ProjectWeb.Models.CookieModel>>(httpCookie.Value) ?? new List<Models.CookieModel>();

                foreach (var item in order)
                {
                    Models.ProductModel productModel = new Models.ProductModel();
                    var warehouse = warehouseService.GetById(item.id);

                    productModel.warehouse_id = warehouse.id;
                    productModel.warehouse_amount = item.amount;
                    productModel.product_name = warehouse.ProductImg.Product.product_name;
                    productModel.product_price = warehouse.ProductImg.Product.product_price ?? 0;
                    productModel.discount = warehouse.ProductImg.Product.discount ?? 0;
                    productModel.product_color = warehouse.ProductImg.color;
                    productModel.size_VN = warehouse.Size.VN ?? 0;
                    productModel.size_US = warehouse.Size.US ?? 0;
                    productModel.size_UK = warehouse.Size.UK ?? 0;
                    cartModel.list_order.Add(productModel);
                }
            }
            return View(cartModel);
        }
        //cap nhat gio hang 
        [HttpPost]
        public ActionResult Index(ProjectWeb.Models.CartModel model)
        {
            HttpCookie httpCookie = HttpContext.Request.Cookies["order"];
            ProjectWeb.Service.WarehouseService warehouseService = new Service.WarehouseService();
            //double timeout = DateTime.Now.AddYears(30));
            List<ProjectWeb.Models.CookieModel> cookieModel = model.list_order.Select(x => new ProjectWeb.Models.CookieModel()
            {
                id = x.warehouse_id,
                amount = x.warehouse_amount
            }).ToList();
           
            ProjectWeb.Models.CartModel cartModel = new Models.CartModel()
            {
                list_order = new List<Models.ProductModel>()
            };
            //hien du lieu ra view
                foreach (var item in cookieModel)
                {
                    Models.ProductModel productModel = new Models.ProductModel();
                    var warehouse = warehouseService.GetById(item.id);

                    productModel.warehouse_id = warehouse.id;
                    productModel.warehouse_amount = item.amount;
                    productModel.product_name = warehouse.ProductImg.Product.product_name;
                    productModel.product_price = warehouse.ProductImg.Product.product_price ?? 0;
                    productModel.discount = warehouse.ProductImg.Product.discount ?? 0;
                    productModel.product_color = warehouse.ProductImg.color;
                    productModel.size_VN = warehouse.Size.VN ?? 0;
                    productModel.size_US = warehouse.Size.US ?? 0;
                    productModel.size_UK = warehouse.Size.UK ?? 0;
                    cartModel.list_order.Add(productModel);
                }
            //update cookie
            //httpCookie.Expires = DateTime.Now.AddHours(timeout);

            httpCookie.Value = JsonConvert.SerializeObject(cookieModel);
            HttpContext.Response.Cookies.Add(httpCookie);

            //change db
            //var old = cookieService.GetById(model.Id);
            //old.ExpiredDate = DateTime.Now.AddHours(timeout);
            //int i = 0;
            //List<ProjectWeb.Cart> carts = model.Carts.ToList();
            //foreach (var item in old.Carts.Where(x=>x.Status==1))
            //{
            //    if (item.Warehouse.Amount - cartService.GetAmount(item.WarehouseId.Value) + item.Amount >= carts[i].Amount)
            //    {
            //        item.Amount = carts[i].Amount;
            //        item.UpdateDate = DateTime.Now;
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("error", @item.Warehouse.ProductImg.Product.Name.ToString() + " - " + @item.Warehouse.ProductImg.Color.ToString() + " VN : " + @item.Warehouse.Size.VN.ToString() + " - US : " + @item.Warehouse.Size.US.ToString() + " - UK : " + @item.Warehouse.Size.UK.ToString() + " không đủ số lượng. ");
            //    }
            //    i++;
            //    if (i > carts.Count)
            //        break;
            //}
            if (ModelState.IsValid)
            {
                //if (cookieService.Update(old))
                // {
                ModelState.AddModelError("success", "Cập nhật gió hàng thành công.");
                //}
                //else
                //{
                //    ModelState.AddModelError("error", "Cập nhật gió hàng thất bại.");
                //}
            }
            return View(cartModel);
        }
        [HttpGet]
        public ActionResult Order()
        {
            HttpCookie httpCookie = HttpContext.Request.Cookies["order"];
            ProjectWeb.Service.WarehouseService warehouseService = new Service.WarehouseService();

            List<ProjectWeb.OrderDetail> listOrderDetail = new List<ProjectWeb.OrderDetail>();
            var orders = JsonConvert.DeserializeObject<List<ProjectWeb.Models.CookieModel>>(httpCookie.Value) ?? new List<Models.CookieModel>();
            ProjectWeb.Models.CartModel cartModel = new Models.CartModel()
            {
                list_order = new List<Models.ProductModel>()
            };
            foreach (var item in orders)
            {
                ProjectWeb.OrderDetail productModel = new ProjectWeb.OrderDetail();
                var warehouse = warehouseService.GetById(item.id);
                productModel.warehouse_id = warehouse.id;
                productModel.amount = item.amount;
                var price = Math.Ceiling(Convert.ToDouble(warehouse.ProductImg.Product.product_price.Value) / 1000 * (100 - warehouse.ProductImg.Product.discount ?? 0) / 100);

                productModel.product_price = Convert.ToInt32(price * 1000);
                listOrderDetail.Add(productModel);
            }
            ProjectWeb.Order order = new ProjectWeb.Order();
            order.discount = 0;
            order.OrderDetails = listOrderDetail;
            return View(order);
        }

        [HttpPost]
        public ActionResult Order(ProjectWeb.Order model)
        {

            ProjectWeb.Service.PromotionService promotionService = new ProjectWeb.Service.PromotionService();
            ProjectWeb.Promotion promotion = null;
            if (string.IsNullOrEmpty(model.buyer_name))
            {
                ModelState.AddModelError("buyer_name", "Xin mời nhập họ tên.");
            }
            if (string.IsNullOrEmpty(model.phone))
            {
                ModelState.AddModelError("phone", "Xin mời nhập số điện thoại nhận hàng.");
            }
            else if (model.phone.Length != 10 || model.phone[0] != '0')
            {
                ModelState.AddModelError("phone", "Số điện thoại không phù hợp");
            }
            if (model.type.HasValue)
            {
                if (!ViewConfig.ListPay.Contains(model.type.Value))
                {
                    ModelState.AddModelError("type", "Vui lòng chọn hình thức thanh toán.");
                }
            }
            else
            {
                ModelState.AddModelError("type", "Vui lòng chọn hình thức thanh toán.");
            }
            if (string.IsNullOrEmpty(model.addressto))
            {
                ModelState.AddModelError("addressto", "Xin mời nhập số địa chỉ nhận hàng.");
            }

            if (!string.IsNullOrEmpty(model.keycode))
            {
                promotion = promotionService.GetByKeyCode(model.keycode);
                if (promotion == null)
                {
                    model.discount = 0;
                    model.keycode = "";
                    ModelState.AddModelError("keycode", "Mã giảm giá không tồn tại hoặc đã hết.");
                }
                else
                {
                    if (promotion.prom_amount.HasValue)
                        promotion.prom_amount -= 1;
                    model.discount = promotion.prom_discount.Value;
                }
            }
            else
            {
                model.discount = 0;
            }

            //ViewBag.Province = new ProjectWeb.Service.ExtendService().GetAddProvice();
            ProjectWeb.Service.OrderService orderService = new ProjectWeb.Service.OrderService();
            //ProjectWeb.Service.CookieService cookieService = new ProjectWeb.Service.CookieService();

            //ProjectWeb.Cookie cookies = new ProjectWeb.Cookie();
            ProjectWeb.Service.WarehouseService warehouseService = new ProjectWeb.Service.WarehouseService();
            List<ProjectWeb.OrderDetail> list = new List<ProjectWeb.OrderDetail>();
            HttpCookie httpCookie = HttpContext.Request.Cookies["order"];
            if (httpCookie != null)
            {
                var orders = JsonConvert.DeserializeObject<List<ProjectWeb.Models.CookieModel>>(httpCookie.Value) ?? new List<Models.CookieModel>();
                foreach (var item in orders)
                {
                    var warehouse = warehouseService.GetById(item.id);
                    var price = Math.Ceiling(Convert.ToDouble(warehouse.ProductImg.Product.product_price.Value) / 1000 * (100 - warehouse.ProductImg.Product.discount.Value) / 100);

                    list.Add(new OrderDetail()
                    {
                        discount = warehouse.ProductImg.Product.discount.Value,
                        is_delete = false,
                        product_id = warehouse.ProductImg.Product.product_id,
                        amount = item.amount,
                        warehouse_id = item.id,
                        product_price = Convert.ToInt32(price * 1000),
                    });
                };
            }

            model.OrderDetails = list;
            model.total = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(model.OrderDetails.Sum(x => x.product_price.Value * x.amount.Value) / 1000) * (100 - model.discount.Value) / 100)) * 1000;
            Random r = new Random();
            int k = r.Next(1000, 9999);
            model.order_code = "THESHOESSHOP" + DateTime.Now.ToString("yyyyMMddHHmmss") + k;
            model.order_createdate = DateTime.Now;
            model.order_status = (int)OrderConfig.Status.WAIT;
            model.discount = model.discount ?? 0;
            model.customer_pay = 0;

            if (ModelState.IsValid)
            {              
                if (orderService.Insert(model))
                {
                    warehouseService.Update();
                    promotionService.Update();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    ModelState.AddModelError("success", "Đặt hàng thành công.");
                    httpCookie.Value = "";
                    HttpContext.Response.Cookies.Add(httpCookie);
                    return View(new ProjectWeb.Order());
                }
                else
                {
                    ModelState.AddModelError("error", "Đặt hàng thất bại.");
                    return View(model);
                }
            }
            return View(model);
        }
        public bool RemoveProductFromCart(int id)
        {
            HttpCookie httpCookie = HttpContext.Request.Cookies["order"];
            ProjectWeb.Service.WarehouseService warehouseService = new Service.WarehouseService();
            if (httpCookie != null)
            {
                var orders = JsonConvert.DeserializeObject<List<ProjectWeb.Models.CookieModel>>(httpCookie.Value) ?? new List<Models.CookieModel>();
                var removeitem = orders.FirstOrDefault(x => x.id == id);
                if (removeitem != null)
                {
                    orders.Remove(removeitem);
                }
                httpCookie.Value = JsonConvert.SerializeObject(orders);
                HttpContext.Response.Cookies.Add(httpCookie);
            }         
            return true;
        }
        [HttpPost]
        public ActionResult ApplyPromotion(ProjectWeb.Order model)
        {
            if (string.IsNullOrEmpty(model.buyer_name))
            {
                ModelState.AddModelError("buyer_name", "Xin mời nhập họ tên.");
            }
            if (string.IsNullOrEmpty(model.phone))
            {
                ModelState.AddModelError("phone", "Xin mời nhập số điện thoại nhận hàng.");
            }
            else if (model.phone.Length != 10)
            {
                ModelState.AddModelError("phone", "Số điện thoại không khả dụng.");
            }
            if (string.IsNullOrEmpty(model.addressto))
            {
                ModelState.AddModelError("addressto", "Xin mời nhập số địa chỉ nhận hàng.");
            }

            //ViewBag.Province = new ProjectWeb.Service.ExtendService().GetAddProvice();
            ProjectWeb.Service.OrderService orderService = new ProjectWeb.Service.OrderService();
            //ProjectWeb.Service.CookieService cookieService = new ProjectWeb.Service.CookieService();
            //HttpCookie httpCookie = HttpContext.Request.Cookies["key"];
            //ProjectWeb.Cookie cookies = new ProjectWeb.Cookie();

            ProjectWeb.Service.WarehouseService warehouseService = new ProjectWeb.Service.WarehouseService();
            List<ProjectWeb.OrderDetail> list = new List<ProjectWeb.OrderDetail>();
            HttpCookie httpCookie = HttpContext.Request.Cookies["order"];
            if (httpCookie != null)
            {
                var orders = JsonConvert.DeserializeObject<List<ProjectWeb.Models.CookieModel>>(httpCookie.Value) ?? new List<Models.CookieModel>();
                foreach (var item in orders)
                {
                    var warehouse = warehouseService.GetById(item.id);
                    var price = Math.Ceiling(Convert.ToDouble(warehouse.ProductImg.Product.product_price.Value) / 1000 * (100 - warehouse.ProductImg.Product.discount.Value) / 100);

                    list.Add(new OrderDetail()
                    {
                        discount = warehouse.ProductImg.Product.discount.Value,
                        is_delete = false,
                        product_id = warehouse.ProductImg.Product.product_id,

                        amount = item.amount,
                        warehouse_id = item.id,
                        product_price = Convert.ToInt32(price * 1000),
                    });
                };
            }
            //if (ModelState.IsValid)
            //{
                model.OrderDetails = list;
                if (!string.IsNullOrEmpty(model.keycode))
                {
                    ProjectWeb.Promotion promotion = new ProjectWeb.Service.PromotionService().GetByKeyCode(model.keycode);
                    if (promotion == null)
                    {
                        model.discount = 0;
                        model.keycode = "";
                        ModelState.AddModelError("keycode", "Mã giảm giá không tồn tại hoặc đã hết.");
                    }
                    else
                    {
                        if (orderService.CheckPhoneUserKey(model.phone, model.keycode))
                        {
                            ModelState.AddModelError("keycode", "Mã giảm giá đã sử dụng.");
                        }
                        else
                            model.discount = promotion.prom_discount.Value;
                    }
                }
           // }
            return View("Order", model);
        }

        public ActionResult OrderList(string phone, int page = 1, int size = 5)
        {
            ProjectWeb.Service.OrderService orderService = new ProjectWeb.Service.OrderService();
            ViewBag.Phone = phone;
            ViewBag.Page = page;
            ViewBag.Size = size;

            int skip = (page - 1) * size;
            List<ProjectWeb.Order> orders = orderService.GetAllByPhone(phone, skip, size);
            int count = orderService.CountAllByPhone(phone);
            ViewBag.Count = count;
            StaticPagedList<ProjectWeb.Order> pageList = new StaticPagedList<ProjectWeb.Order>(orders, page, size, count);
            return View(pageList);
        }
    }
}