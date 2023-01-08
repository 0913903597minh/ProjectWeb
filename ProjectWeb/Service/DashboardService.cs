using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectWeb.Configuration;

namespace ProjectWeb.Service
{
    public class DashboardService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public double RevenueMonth(DateTime dateTime)
        {
            try
            {
                return db.Orders.Where(x => x.order_status == (int)Configuration.OrderConfig.Status.FINISH && x.order_updatedate.Value.Month == dateTime.Month).Sum(x => x.total.Value);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public double RevenueDay(DateTime dateTime)
        {
            try
            {
                return db.Orders.Where(x => x.order_status == (int)Configuration.OrderConfig.Status.FINISH && x.order_updatedate.Value.Day == dateTime.Day).Sum(x => x.total.Value);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public double RevenueYear(DateTime dateTime)
        {
            try
            {
                return db.Orders.Where(x => x.order_status == (int)Configuration.OrderConfig.Status.FINISH && x.order_updatedate.Value.Year == dateTime.Year).Sum(x => x.total.Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public List<ModelReport> TopBestSale()
        {
            try
            {
                DateTime td = DateTime.Now.AddMonths(-3);
              
                var list = db.OrderDetails.Where(x => x.is_delete != true && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH && x.Order.order_createdate > td).ToList();
                var a = list.GroupBy(x => x.product_id.Value).Select(y => new ModelReport
                {
                    id = y.Key,
                    amount = y.Sum(x => x.amount.Value),
                }).ToList();

                return a.ToList().OrderByDescending(x=>x.amount).Skip(0).Take(3).ToList();
            }
            catch (Exception e)
            {
                return new List<ModelReport>();
            }
        }
        public List<ModelReport> TopBadSale()
        {
            try
            {
                DateTime td = DateTime.Now.AddMonths(-3);
           
                var list = db.OrderDetails.Where(x => (x.is_delete != true) && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH && x.Order.order_createdate > td).ToList();
                var a = list.GroupBy(x => x.product_id.Value).Select(y => new ModelReport
                {
                    id = y.Key,
                    amount = y.Sum(x => x.amount.Value),
                }).ToList();
                return a.OrderBy(x => x.amount).Skip(0).Take(3).ToList();
            }
            catch (Exception e)
            {
                return new List<ModelReport>();
            }
        }
        public List<Product> Stocking()
        {
            try
            {
                return db.Products.Where(x => (x.is_delete != true) && x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                && x.ProductImgs.Count(y=> (y.is_delete != true) && y.Warehouses.Count>0) >0).ToList().OrderByDescending(x=>x.ProductImgs.Sum(y=>y.Warehouses.Sum(m=>m.amount))).Skip(0).Take(5).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public List<Product> OutOfStock()
        {
            try
            {
                return db.Products.Where(x => (x.is_delete != true) && x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                && x.ProductImgs.Count(y => (y.is_delete != true) && y.Warehouses.Count > 0) > 0).ToList().OrderBy(x => x.ProductImgs.Sum(y => y.Warehouses.Sum(m => m.amount))).Skip(0).Take(5).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public List<Product> NotImport()
        {
            try
            {
                return db.Products.Where(x => (x.is_delete != true) && x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                && x.ProductImgs.Count(y => (y.is_delete != true) && (y.Warehouses.Count<1 || y.Warehouses.Count(m=>m.amount==0 && m .sold==0)>0)) > 0).ToList().OrderByDescending(x => x.create_date).Skip(0).Take(5).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public List<ModelReport> TopBadSize()
        {
            try
            {
                DateTime td = DateTime.Now.AddMonths(-3);

                var list = db.OrderDetails.Where(x => (x.is_delete != true) && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH && x.Order.order_createdate > td).ToList();
                var a = list.GroupBy(x => x.Warehouse.Size).Select(y => new ModelReport
                {
                    id = y.Key.size_id,
                    amount = y.Sum(x => x.amount.Value),
                }).ToList();
                return a.OrderBy(x => x.amount).Skip(0).Take(3).ToList();
            }
            catch (Exception e)
            {
                return new List<ModelReport>();
            }
        }
        public List<ModelReport> TopBestSize()
        {
            try
            {
                DateTime td = DateTime.Now.AddMonths(-3);

                var list = db.OrderDetails.Where(x => (x.is_delete != true) && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH && x.Order.order_createdate > td).ToList();
                var a = list.GroupBy(x => x.Warehouse.Size).Select(y => new ModelReport
                {
                    id = y.Key.size_id,
                    amount = y.Sum(x => x.amount.Value),
                }).ToList();
                return a.ToList().OrderByDescending(x => x.amount).Skip(0).Take(3).ToList();
            }
            catch (Exception e)
            {
                return new List<ModelReport>();
            }
        }
    }
  
    public class ModelReport{
       public int id { get; set; }
        public int amount { get; set; }
    }

}
