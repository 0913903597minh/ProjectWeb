using ProjectWeb;
using ProjectWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class ViewService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public List<Product> GetAllTopSale()
        {
            try
            {
                var a = db.Warehouses.Where(x => x.is_delete != true).GroupBy(x => x.ProductImg.Product.product_id).Select(y => new ModelReport
                {
                    id = y.Key,
                    amount = y.Sum(x => x.sold.Value),
                }).ToList();
                List<int> listid = a.Select(x => x.id).ToList();
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && listid.Contains(x.product_id)
                 && (x.is_delete != true ) && x.ProductImgs.Count(m => (m.is_delete != true)
                 && m.Warehouses.Count(n => n.amount > 0) > 0) > 0).ToList().OrderByDescending(x => x.ProductImgs.Where(y => (y.is_delete != true)).Sum(y => y.Warehouses.Sum(z => z.sold))).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllTopSale()
        {
            try
            {
                var a = db.Warehouses.Where(x => x.is_delete != true).GroupBy(x => x.ProductImg.Product.product_id).Select(y => new ModelReport
                {
                    id = y.Key,
                    amount = y.Sum(x => x.sold.Value),
                }).ToList();
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                && a.Select(y => y.id).Contains(x.product_id)
                && (x.is_delete != true) && x.ProductImgs.Count(m => (m.is_delete != true)
                && m.Warehouses.Count(n => n.amount > 0) > 0) > 0);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllTopSale(int skip, int size)
        {
            try
            {
                var a = db.Warehouses.Where(x =>x.is_delete != true).GroupBy(x => x.ProductImg.Product.product_id).Select(y => new ModelReport
                {
                    id = y.Key,
                    amount = y.Sum(x => x.sold.Value),
                }).ToList();
                List<int> listid = a.Select(x => x.id).ToList();
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                && listid.Contains(x.product_id)
                && (x.is_delete != true) && x.ProductImgs.Count(m => (m.is_delete == false || m.is_delete == null)
                && m.Warehouses.Count(n => n.amount > 0) > 0) > 0).ToList().OrderByDescending(x => x.ProductImgs.Where(y=> (y.is_delete == false || y.is_delete == null)).Sum(y=>y.Warehouses.Sum(z=>z.sold))).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllTopSale(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.product_name.Contains(keysearch) && (typeid != -1 ? x.type == typeid : true)
                 && (brandid != -1 ? x.brand_id == brandid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllTopSale(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && x.ProductImgs.Count > 0 && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)
                 && (typeid != -1 ? x.type == typeid : true)
                 ).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public List<Product> GetAllProductNew()
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && x.ProductImgs.Count > 0).OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductNew()
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && x.ProductImgs.Count > 0);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductNew(int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductNew(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)
                 && (typeid != -1 ? x.type == typeid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductNew(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)
                 && (typeid != -1 ? x.type == typeid : true)).OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public List<Product> GetAllProductDiscount()
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.discount > 0).ToList().OrderBy(x => x.discount).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductDiscount()
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && x.discount > 0 && x.ProductImgs.Count > 0);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductDiscount(int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.discount > 0).ToList().OrderBy(x => x.discount).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductDiscount(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.discount > 0 && x.product_name.Contains(keysearch) && (typeid != -1 ? x.type == typeid : true)
                 && (brandid != -1 ? x.brand_id == brandid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductDiscount(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && x.discount > 0 && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)).ToList().OrderBy(x => x.discount).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }

        //Male
        public List<Product> GetAllProductMale()
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.MALE).ToList().OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductMale()
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.MALE);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductMale(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.MALE && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductMale(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.MALE && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductMale(int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.MALE).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }

        //Female
        public List<Product> GetAllProductFemale()
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.FEMALE).ToList().OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductFemale()
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.FEMALE);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductFemale(int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.FEMALE).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductFemale(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.FEMALE && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductFemale(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.FEMALE && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }

        //Boy
        public List<Product> GetAllProductBoy()
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.BOY).ToList().OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductBoy()
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.BOY);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductBoy(int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.BOY).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductBoy(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.BOY && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductBoy(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.BOY && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }

        //Girl
        public List<Product> GetAllProductGirl()
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.GIRL).ToList().OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductGirl()
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.GIRL);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductGirl(int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.ProductImgs.Count > 0
                 && (x.is_delete != true) && x.type == (int)Configuration.ProductConfig.Type.GIRL).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllProductGirl(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.GIRL && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductGirl(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.type == (int)Configuration.ProductConfig.Type.GIRL && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }

        // Brand
        public int CountAllProductByBrand(string keysearch, int typeid, int brandid)
        {
            try
            {
                return db.Products.Count(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllProductByBrand(string keysearch, int typeid, int brandid, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE
                 && (x.is_delete != true) && (typeid != -1 ? x.type == typeid : true) && x.ProductImgs.Count > 0
                 && x.product_name.Contains(keysearch) && (brandid != -1 ? x.brand_id == brandid : true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public List<Product> GetAllProductSimilar(int id, int brandid, int type, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.product_status == (int)Configuration.ProductConfig.Status.ACTIVE && x.brand_id != id
                 && (x.is_delete != true) && (x.type == type || x.brand_id == brandid) && x.ProductImgs.Count > 0).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }

        // List View product
        public List<Product> GetAllByAllKey(string key, int brand, int sex, string sort,int sizeP, int skip, int size)
        {
            try
            {
                List<Product> products = db.Products.Where(x => x.product_name.Contains(key) && (brand != -1 ? x.brand_id == brand : true)
                && (sex != -1 ? x.type == sex : true) && x.product_status == 1 && (x.is_delete != true)
                && x.ProductImgs.Count(m => m.status == 1 && (m.is_delete == false || m.is_delete == null)) > 0).ToList();
                if (sizeP != -1)
                {
                    List<int> list = db.Warehouses.Where(x => (x.is_delete != true) && x.status == 1
                    && x.amount > 0 && x.size_id==sizeP).Select(x => x.ProductImg.product_id.Value).ToList();
                    products = products.Where(x => list.Contains(x.product_id)).ToList();
                }
                if (sort == Configuration.ViewConfig.TOPSALE)
                {
                    var a = db.OrderDetails.Where(x => (x.is_delete != true) && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH).GroupBy(x => x.product_id).Select(y => new ModelReport
                    {
                        id = y.Key.Value,
                        amount = y.Sum(x => x.amount.Value),
                    }).ToList();
                    List<int> listid = a.Select(x => x.id).ToList();
                    products = products.Where(x=>listid.Contains(x.product_id)).ToList().OrderByDescending(x => x.ProductImgs.Where(y =>listid.Contains(y.productimg_id) && (y.is_delete != true)).Sum(y => y.Warehouses.Sum(z => z.sold))).ToList();
                }
                if (sort == Configuration.ViewConfig.PRODUCTNEW)
                {
                    products = products.ToList().OrderByDescending(x => x.ProductImgs.Max(m => m.create_date)).ToList();
                }
                if (sort == Configuration.ViewConfig.DISCOUNT)
                {
                    products = products.ToList().OrderByDescending(x => x.discount > 0).ToList();
                }              
                return products.Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllByAllKey(string key, int brand, int sex, string sort, int sizeP)
        {
            try
            {
               var products = db.Products.Where(x => x.product_name.Contains(key) && (brand != -1 ? x.brand_id == brand : true)
                  && (sex != -1 ? x.type == sex : true) && x.product_status == 1 && (x.is_delete != true)
                  && x.ProductImgs.Count(m => m.status == 1 && (m.is_delete == false || m.is_delete == null)) > 0);
                if (sizeP != -1)
                {
                    List<int> list = db.Warehouses.Where(x => (x.is_delete != true) && x.status == 1
                    && x.amount > 0 && x.size_id == sizeP).Select(x => x.ProductImg.product_id.Value).ToList();
                    products = products.Where(x => list.Contains(x.product_id));
                }
                if (sort == Configuration.ViewConfig.TOPSALE)
                {
                    var a = db.OrderDetails.Where(x => (x.is_delete != true) && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH).GroupBy(x => x.product_id).Select(y => new ModelReport
                    {
                        id = y.Key.Value,
                        amount = y.Sum(x => x.amount.Value),
                    }).ToList();
                    List<int> listid = a.Select(x => x.id).ToList();
                    products = products.Where(x => listid.Contains(x.product_id)).OrderByDescending(x => x.ProductImgs.Where(y => listid.Contains(y.productimg_id) && (y.is_delete != true)).Sum(y => y.Warehouses.Sum(z => z.amount)));
                }
                if (sort == Configuration.ViewConfig.PRODUCTNEW)
                {
                    products = products.OrderByDescending(x => x.ProductImgs.Max(m => m.create_date));
                }
                if (sort == Configuration.ViewConfig.DISCOUNT)
                {
                    products = products.OrderByDescending(x => x.discount > 0);
                }              
                return products.Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Product> GetAllByAllKey(string key, List<int> brand, int sex, string sort, int sizeP, int skip, int size)
        {
            try
            {
                List<Product> products = db.Products.Where(x => x.product_name.Contains(key) && x.brand_id.HasValue
                && (sex != -1 ? x.type == sex : true) && x.product_status == 1 && (x.is_delete != true)
                && x.ProductImgs.Count(m => m.status == 1 && (m.is_delete == false || m.is_delete == null)) > 0).ToList();
                brand = brand ?? new List<int>();
                if(brand.Count > 0)
                {
                    products = products.Where(x => brand.Contains(x.brand_id.Value)).ToList();
                }
                if (sizeP != -1)
                {
                    List<int> list = db.Warehouses.Where(x => (x.is_delete != true) && x.status == 1
                    && x.amount > 0 && x.size_id == sizeP).Select(x => x.ProductImg.product_id.Value).ToList();
                    products = products.Where(x => list.Contains(x.product_id)).ToList();
                }
                if (sort == Configuration.ViewConfig.TOPSALE)
                {
                    var a = db.OrderDetails.Where(x => (x.is_delete != true) && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH).GroupBy(x => x.product_id).Select(y => new ModelReport
                    {
                        id = y.Key.Value,
                        amount = y.Sum(x => x.amount.Value),
                    }).ToList();
                    List<int> listid = a.Select(x => x.id).ToList();
                    products = products.Where(x => listid.Contains(x.product_id)).ToList().OrderByDescending(x => x.ProductImgs.Where(y => listid.Contains(y.productimg_id) && (y.is_delete != true)).Sum(y => y.Warehouses.Sum(z => z.sold))).ToList();
                }
                if (sort == Configuration.ViewConfig.PRODUCTNEW)
                {
                    products = products.ToList().OrderByDescending(x => x.ProductImgs.Max(m => m.create_date)).ToList();
                }
                if (sort == Configuration.ViewConfig.DISCOUNT)
                {
                    products = products.ToList().OrderByDescending(x => x.discount > 0).ToList();
                }               
                return products.Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }
        public int CountAllByAllKey(string key, List<int> brand, int sex, string sort, int sizeP)
        {
            try
            {
                var products = db.Products.Where(x => x.product_name.Contains(key) && x.brand_id.HasValue
               && (sex != -1 ? x.type == sex : true) && x.product_status == 1 && (x.is_delete != true)
               && x.ProductImgs.Count(m => m.status == 1 && (m.is_delete == false || m.is_delete == null)) > 0);
                brand = brand ?? new List<int>();
                if (brand.Count > 0)
                {
                    products = products.Where(x => brand.Contains(x.brand_id.Value));
                }
                if (sizeP != -1)
                {
                    List<int> list = db.Warehouses.Where(x => (x.is_delete != true) && x.status == 1
                    && x.amount > 0 && x.size_id == sizeP).Select(x => x.ProductImg.product_id.Value).ToList();
                    products = products.Where(x => list.Contains(x.product_id));
                }
                if (sort == Configuration.ViewConfig.TOPSALE)
                {
                    var a = db.OrderDetails.Where(x => (x.is_delete != true) && x.Order.order_status == (int)Configuration.OrderConfig.Status.FINISH).GroupBy(x => x.product_id).Select(y => new ModelReport
                    {
                        id = y.Key.Value,
                        amount = y.Sum(x => x.amount.Value),
                    }).ToList();
                    List<int> listid = a.Select(x => x.id).ToList();
                    products = products.Where(x => listid.Contains(x.product_id)).OrderByDescending(x => x.ProductImgs.Where(y => listid.Contains(y.productimg_id) && (y.is_delete != true)).Sum(y => y.Warehouses.Sum(z => z.amount)));
                }
                if (sort == Configuration.ViewConfig.PRODUCTNEW)
                {
                    products = products.OrderByDescending(x => x.ProductImgs.Max(m => m.create_date));
                }
                if (sort == Configuration.ViewConfig.DISCOUNT)
                {
                    products = products.OrderByDescending(x => x.discount > 0);
                }               
                return products.Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
