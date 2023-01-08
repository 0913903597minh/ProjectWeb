using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class ProductService
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public bool Insert(Product model)
        {
            try
            {
                model.product_status = 1;
                model.create_date = DateTime.Now;
                db.Products.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(Product model)
        {
            try
            {
                Product product = GetById(model.product_id);
                product.avatar_url = model.avatar_url;
                product.product_name = model.product_name;
                product.product_describe = model.product_describe;
                product.product_status = model.product_status;
                product.brand_id = model.brand_id;
                product.update_date = DateTime.Now;
                product.discount = model.discount;
                product.product_price = model.product_price;
                product.type = model.type;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateStatus(Product model)
        {
            try
            {
                Product product = GetById(model.product_id);
                product.product_status = model.product_status;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateSold(Product model)
        {
            try
            {
                Product product = GetById(model.product_id);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                Product product = GetById(id);
                product.is_delete = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Product GetById(int id)
        {
            try
            {
                return db.Products.Find(id);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public List<Product> GetAll()
        {
            try
            {
                return db.Products.Where(x => x.is_delete == false || x.is_delete == null).OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<Product> GetAll(int skip, int size)
        {
            try
            {
                return db.Products.Where(x => x.is_delete == false || x.is_delete == null).OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public int CountAll()
        {
            try
            {
                return db.Products.Count(x => x.is_delete == false || x.is_delete == null);
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public List<Product> GetAllByKey(string key, int brandid, int sex, int status, int skip, int size)
        {
            try
            {
                return db.Products.Where(x => (x.is_delete == false || x.is_delete == null) && x.product_name.Contains(key)
                && (sex != -1 ? x.type == sex : true) && (status != -1 ? x.product_status == status : true)
                && (brandid != -1 ? x.brand_id.Value == brandid : true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int CountAllByKey(string key, int brandid, int sex, int status)
        {
            try
            {
                return db.Products.Count(x => (x.is_delete == false || x.is_delete == null) && (status != -1 ? x.product_status == status : true)
                && (brandid != -1 ? x.brand_id.Value == brandid : true) && x.product_name.Contains(key) && (sex != -1 ? x.type == sex : true));
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public bool DiscountProduct(List<int> productid, List<int> brandid, int discount)
        {
            try
            {
                var list = new List<Product>();
                if (productid.Count > 0 && brandid.Count > 0)
                {
                    list = db.Products.Where(x => productid.Contains(x.product_id) || brandid.Contains(x.brand_id.Value)).ToList();
                    list.ForEach(x => x.discount = discount);
                    list.ForEach(x => x.update_date = DateTime.Now);
                    db.SaveChanges();
                    return true;
                }
                if (productid.Count > 0 && brandid.Count < 1)
                {
                    list = db.Products.Where(x => productid.Contains(x.product_id)).ToList();
                    list.ForEach(x => x.discount = discount);
                    list.ForEach(x => x.update_date = DateTime.Now);
                    db.SaveChanges();
                    return true;
                }
                if (productid.Count < 1 && brandid.Count > 0)
                {
                    list = db.Products.Where(x => brandid.Contains(x.brand_id.Value)).ToList();
                    list.ForEach(x => x.discount = discount);
                    list.ForEach(x => x.update_date = DateTime.Now);
                    db.SaveChanges();
                    return true;
                }
                list = db.Products.ToList();
                list.ForEach(x => x.discount = discount);
                list.ForEach(x => x.update_date = DateTime.Now);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckNameAndType(Product model)
        {
            try
            {
                var product = db.Products.FirstOrDefault(x => (x.is_delete == false || x.is_delete == null)
                && x.product_name.Trim().ToLower() == model.product_name.Trim().ToLower() && x.type == model.type && x.product_id != model.product_id);
                if (product != null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
