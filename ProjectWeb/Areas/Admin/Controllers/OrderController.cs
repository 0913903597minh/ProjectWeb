using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(string keysearch, int status = -1, int paid = -1, int type = -1, int page = 1, int size = 10)
        {
            ViewBag.keysearch = keysearch;
            ViewBag.status = status;
            ViewBag.paid = paid;
            ViewBag.type = type;
            ViewBag.page = page;
            ViewBag.size = size;
            return View();
        }
        public ActionResult ListOrder(string keysearch, int status = -1, int paid = -1, int type = -1, int page = 1, int size = 10)
        {
            ViewBag.keysearch = keysearch;
            ViewBag.status = status;
            ViewBag.paid = paid;
            ViewBag.type = type;
            ViewBag.page = page;
            ViewBag.size = size;
            int skip = (page - 1) * size;
            OrderService orderService = new OrderService();
            var list = orderService.GetAllByKey(keysearch, status, type, paid, skip, size);
            //if (list == null)
            //{
            //    list = new List<Order>();
            //}
            var count = orderService.CountAllByKey(keysearch, status, type, paid);
            ViewBag.revenue = orderService.GetTotalMoney(keysearch, status, type, paid);
            ViewBag.count = count;
            StaticPagedList<Order> pagedList = new StaticPagedList<Order>(list, page, size, count);
            return View(pagedList);
        }
        [HttpGet]
        public ActionResult AddOrder()
        {

            Order order = new Order();
            order.total = 0;
            order.customer_pay = 0;
            return View(order);
        }
        [HttpPost]
        public ActionResult AddOrder(Order model)
        {
            model.customer_pay = model.customer_pay ?? 0;
            PromotionService PromotionService = new PromotionService();
            Promotion promotion = null;
          
            OrderService orderService = new OrderService();
            WarehouseService WarehouseService = new WarehouseService();
            if (string.IsNullOrEmpty(model.buyer_name))
            {
                ModelState.AddModelError("BuyerName", "Xin mời nhập họ tên.");
            }
            if (model.type.HasValue)
            {
                if (!Configuration.ViewConfig.ListPay.Contains(model.type.Value))
                {
                    ModelState.AddModelError("Type", "Vui lòng chọn hình thức thanh toán.");
                }
            }
            else
            {
                ModelState.AddModelError("Type", "Vui lòng chọn hình thức thanh toán.");
            }
            if (string.IsNullOrEmpty(model.phone))
            {
                ModelState.AddModelError("Phone", "Xin mời nhập số điện thoại nhận hàng.");
            }
            else 
           if (model.phone.Length != 10 || model.phone[0] != '0')
            {
                ModelState.AddModelError("Phone", "Số điện thoại không phù hợp");
            }
            if (string.IsNullOrEmpty(model.addressto))
            {
                ModelState.AddModelError("AddressTo", "Xin mời nhập số địa chỉ nhận hàng.");
            }
            if (model.OrderDetails == null||model.OrderDetails.Count<1)
            {
                ModelState.AddModelError("error", "Vui lòng thêm sản phẩm");
            }
            foreach (var item in model.OrderDetails)
            {
                Warehouse warehouse = WarehouseService.GetById(item.warehouse_id.Value);
                if (warehouse == null)
                {
                    ModelState.AddModelError("error", "Lỗi");
                    break;
                }
                else
                {
                    int p = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(warehouse.ProductImg.Product.product_price / 1000 * (100 - warehouse.ProductImg.Product.discount) / 100))) * 1000;
                    item.product_price = p;
                }
                double amount = (warehouse.Carts != null ? warehouse.Carts.Where(x => x.status.Value == 1).ToList().Sum(x => x.amount.Value) : 0) - (warehouse.OrderDetails != null ? warehouse.OrderDetails.Where(x => x.Order.order_status == (int)Configuration.OrderConfig.Status.WAIT && (x.is_delete == null || x.is_delete == false) && x.warehouse_id != item.warehouse_id).ToList().Sum(x => x.amount.Value) : 0);
                if (item.amount.Value > warehouse.amount.Value - amount)
                {
                    ModelState.AddModelError("error", "Sản phẩm" + warehouse.ProductImg.Product.product_name.ToString() + " - " + warehouse.ProductImg.color.ToString() + " VN : " + warehouse.Size.VN.ToString() + " - US : " + warehouse.Size.ToString() + " - UK : " + warehouse.Size.UK.ToString() + " chỉ còn " + (warehouse.amount.Value - amount).ToString());
                    break;
                }
                else
                {
                    warehouse.amount -= item.amount.Value;
                    if (model.type == Configuration.ViewConfig.BUYINSTORE_INT)
                    {
                        warehouse.sold += item.amount.Value;
                    }
                }
                if (item.amount.Value < 0)
                {
                    item.amount = 0;
                    ModelState.AddModelError("error", "Số lượng sản phẩm không khả dụng");
                    break;
                }
            }
            if (!string.IsNullOrEmpty(model.keycode))
            {
                promotion = PromotionService.GetByKeyCode(model.keycode);
                if (promotion == null)
                {
                    model.discount = 0;
                    model.keycode = "";
                    ModelState.AddModelError("KeyCode", "Mã giảm giá không tồn tại hoặc đã hết.");
                }
                else
                {
                    if (promotion.prom_amount.HasValue)
                        promotion.prom_amount -= 1;
                    model.discount = promotion.prom_discount.Value;
                }
            }
            model.discount = model.discount ?? 0;
            model.total = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(model.OrderDetails.Sum(x => x.product_price.Value * x.amount.Value) / 1000 * (100 - model.discount.Value) / 100))) * 1000;
            if (model.type == Configuration.ViewConfig.BUYINSTORE_INT)
            {
                model.order_status = (int)Configuration.OrderConfig.Status.FINISH;
                if (model.customer_pay.HasValue)
                {
                    if (model.customer_pay < model.total)
                    {
                        ModelState.AddModelError("error", "Khách trả chưa đủ");
                    }
                }
            }
            else
            {
                model.order_status = (int)Configuration.OrderConfig.Status.CONFIRM;
            }
            if (ModelState.IsValid)
            {
              
                var user = Session["User"] as User;
                model.order_createdate = DateTime.Now;

                Random r = new Random();
                int k = r.Next(1000, 9999);
                model.order_code = "SHOESSHOP" + DateTime.Now.ToString("yyyyMMddHHmmss") + k;
                if (orderService.Insert(model))
                {
                    WarehouseService.Update();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(model);
                }
                ModelState.AddModelError("Error", "Lỗi hệ thống");
                //}
            }
            return View(model);
        }

        public ActionResult UpdateOrder(Order model)
        {
            model.customer_pay = model.customer_pay ?? 0;
          
            OrderService orderService = new OrderService();
            foreach (var item in model.OrderDetails)
            {
                Warehouse warehouse = new WarehouseService().GetById(item.warehouse_id.Value);
                if (warehouse == null)
                {
                    ModelState.AddModelError("error", "Lỗi");
                    break;
                }
                else
                {
                    item.product_price = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(warehouse.ProductImg.Product.product_price / 1000 * (100 - warehouse.ProductImg.Product.discount) / 100)) * 1000);
                }
                double amount = 0;
                if (warehouse.Carts != null)
                {
                    amount = (warehouse.Carts != null ? warehouse.Carts.Where(x => x.status.Value == 1).ToList().Sum(x => x.amount.Value) : 0) - (warehouse.OrderDetails != null ? warehouse.OrderDetails.Where(x => x.Order.order_status == (int)Configuration.OrderConfig.Status.WAIT && (x.is_delete == null || x.is_delete.Value == false) && x.warehouse_id != item.warehouse_id).ToList().Sum(x => x.amount.Value) : 0);
                }
                if (item.amount.Value > warehouse.amount.Value - amount)
                {
                    item.amount = 0;
                    ModelState.AddModelError("error", "Sản phẩm" + warehouse.ProductImg.Product.product_name.ToString() + " - " + warehouse.ProductImg.color.ToString() + " VN : " + warehouse.Size.VN.ToString() + " - US : " + warehouse.Size.US.ToString() + " - UK : " + warehouse.Size.UK.ToString() + " chỉ còn " + (warehouse.amount.Value - amount).ToString());
                    break;
                }
                if (!string.IsNullOrEmpty(model.keycode))
                {
                    Promotion promotion = new PromotionService().GetByKeyCode(model.keycode);
                    if (promotion == null)
                    {
                        model.discount = 0;
                        model.keycode = "";
                        ModelState.AddModelError("KeyCode", "Mã giảm giá không tồn tại hoặc đã hết.");
                    }
                    else
                    {
                        model.discount = promotion.prom_discount.Value;
                    }
                }
                if (item.amount.Value < 0)
                {
                    ModelState.AddModelError("error", "Số lượng sản phẩm không khả dụng");
                    break;
                }
            }
            return View("AddOrder", model);
        }

        [HttpGet]
        public ActionResult AddOrderDetail(string list)
        {
            if (string.IsNullOrEmpty(list))
                list = "0";
            List<string> listDatas = list.Split(',').ToList();
            ViewBag.listDatas = listDatas;
            ViewBag.Warehouse = new WarehouseService().GetAll().Where(x => x.amount > 0).ToList();
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.amount = 1;
            return View(orderDetail);
        }
        [HttpPost]
        public ActionResult AddOrderDetail(OrderDetail model, string list)
        {
            if (string.IsNullOrEmpty(list))
                list = "0";
            List<string> listDatas = list.Split(',').ToList();
            ViewBag.listDatas = listDatas;
            ViewBag.Warehouse = new WarehouseService().GetAll().Where(x => x.amount > 0).ToList();
            Warehouse warehouse = new WarehouseService().GetById(model.warehouse_id.Value);
            double amount = warehouse.Carts.Where(x => x.status.Value == 1).ToList().Sum(x => x.amount.Value);
            if (listDatas.Contains(model.warehouse_id.ToString()))
            {
                ModelState.AddModelError("warehouse_id", "Sản phẩm đã có trong giỏ hàng vui lòng thực hiện thao tác trên giỏ");
            }
            else
            {
                if (model.amount.HasValue)
                {
                    if (model.amount.Value > warehouse.amount.Value - amount)
                    {
                        ModelState.AddModelError("amount", "Sản phẩm chỉ còn " + (warehouse.amount.Value - amount).ToString() + " trong kho.");
                    }
                }
                else
                {
                    ModelState.AddModelError("amount", "Vui lòng nhập số lượng");
                }
            }


            if (ModelState.IsValid)
            {
                model.product_price = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(warehouse.ProductImg.Product.product_price / 1000 * (100 - warehouse.ProductImg.Product.discount) / 100)) * 1000);
                Response.StatusCode = (int)HttpStatusCode.Created;
                return PartialView("trAddOrderDetail", model);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
          
            return View(new OrderService().GetById(id));
        }
        [HttpPost]
        public ActionResult Detail(Order model)
        {
          
            OrderService orderService = new OrderService();
            Order order = orderService.GetById(model.order_id);
            if (order.order_status == (int)Configuration.OrderConfig.Status.CANCEL)
            {
                return View(order);
            }
            if (!Configuration.OrderConfig.StatusToDictionaryHTML.ContainsKey(model.order_status.Value))
            {
                ModelState.AddModelError("error", "Lỗi");
                return View(order);
            }
            if (order.order_status == (int)Configuration.OrderConfig.Status.FINISH && model.order_status == (int)Configuration.OrderConfig.Status.CONFIRM)
            {
                ModelState.AddModelError("error", "Lỗi");
                return View(order);
            }
            if (order.order_status != (int)Configuration.OrderConfig.Status.WAIT && model.order_status == (int)Configuration.OrderConfig.Status.WAIT)
            {
                ModelState.AddModelError("error", "Lỗi");
                return View(order);
            }
            // Chờ => Xác nhận 
            if (order.order_status == (int)Configuration.OrderConfig.Status.WAIT && model.order_status == (int)Configuration.OrderConfig.Status.CONFIRM)
            {
                //Trừ vào kho
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.amount = x.Warehouse.amount - x.amount);
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.update_date = DateTime.Now);
                order.order_status = model.order_status;
                order.refuse = model.refuse;
                order.order_updatedate = DateTime.Now;
                if (orderService.Update(order))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(order);
                }
                return View(order);
            }
            //Xác nhận => hủy
            if (order.order_status == (int)Configuration.OrderConfig.Status.CONFIRM && model.order_status == (int)Configuration.OrderConfig.Status.CANCEL)
            {
                //Cộng lại vào kho
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.amount = x.Warehouse.amount + x.amount);
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.update_date = DateTime.Now);
                order.order_status = model.order_status;
                order.refuse = model.refuse;
                order.order_updatedate = DateTime.Now;
                if (orderService.Update(order))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(order);
                }
                return View(order);
            }
            // Xác nhận => hoàn thành
            if (order.order_status == (int)Configuration.OrderConfig.Status.CONFIRM && model.order_status == (int)Configuration.OrderConfig.Status.FINISH)
            {
                //Cộng số lượng đã bán
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.sold += x.amount);
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.update_date = DateTime.Now);
                order.order_status = model.order_status;
                order.refuse = model.refuse;
                order.order_updatedate = DateTime.Now;
                if (orderService.Update(order))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(order);
                }
                return View(order);
            }
            // hoàn thành => hủy
            if (order.order_status == (int)Configuration.OrderConfig.Status.FINISH && model.order_status == (int)Configuration.OrderConfig.Status.CANCEL)
            {
                //Trừ số lượng đã bán
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.sold -= x.amount);
                //Cộng lại vào kho
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.amount += x.amount);
                order.OrderDetails.ToList().ForEach(x => x.Warehouse.update_date = DateTime.Now);

                order.order_status = model.order_status;
                order.refuse = model.refuse;
                order.order_updatedate = DateTime.Now;
                if (orderService.Update(order))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(order);
                }
                return View(order);
            }
            //đơn hàng đang chờ
            if (order.order_status == (int)Configuration.OrderConfig.Status.WAIT)
            {
                order.order_status = model.order_status;
                order.total= model.OrderDetails.Where(x => x.is_delete == false || x.is_delete == null).Sum(m => m.product_price.Value * m.amount.Value);
            }
            if (ModelState.IsValid)
            {
                order.refuse = model.refuse;
                order.order_updatedate = DateTime.Now;
                if (orderService.Update(order))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(order);
                }
                return View(order);
            }
            return View(order);
        }
        public bool RemoveProductInOrder(int id)
        {
            return true;
        }
        public ActionResult ListOrderDetail(int id)
        {
            return View(new OrderService().GetById(id));
        }
        public FileResult Export()
        {
            FileInfo file = new FileInfo(Server.MapPath(@"/Template/ListOrder.xlsx"));
            ExcelPackage package = new ExcelPackage(file);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorkbook workbook = package.Workbook;

            ExcelWorksheet sheet = workbook.Worksheets[0];
            int stt = 1;
            int startrow = 2;
            List<Order> orders = new OrderService().GetAll();
            foreach (var item in orders)
            {
                sheet.Cells[startrow, 1].Value = stt;
                sheet.Cells[startrow, 2].Value = item.order_code;
                sheet.Cells[startrow, 3].Value = item.phone;
                sheet.Cells[startrow, 4].Value = item.buyer_name;
                sheet.Cells[startrow, 5].Value = item.addressto;
                sheet.Cells[startrow, 6].Value = Configuration.OrderConfig.StatusToDictionaryHTML[item.order_status.Value];
                sheet.Cells[startrow, 7].Value = item.total;
                startrow++;
                stt++;
            }
            var modelTable = sheet.Cells["A2:G" + (startrow - 1)];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            modelTable.AutoFitColumns();
            string filename = "BaoCao_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
        //public FileResult Export(string keysearch="",int status=-1)
        //{
        //    return File();
        //}
        public bool Paid(int id)
        {
            OrderService orderService = new OrderService();
            Order order = orderService.GetById(id);
            if (order.order_status == (int)Configuration.OrderConfig.Status.WAIT)
            {
                order.order_status = (int)Configuration.OrderConfig.Status.CONFIRM;
                //order.UpdateDate = DateTime.Now;
                order.customer_pay = order.total;
                order.OrderDetails.Where(x => (x.is_delete == false && x.is_delete == null)).ToList().ForEach(x => x.Warehouse.amount -= x.amount);
                order.OrderDetails.Where(x => (x.is_delete == false && x.is_delete == null)).ToList().ForEach(x => x.Warehouse.update_date = DateTime.Now);
                return orderService.Update();
            }
            else
            {
                return false;
            }               
        }
    }
}