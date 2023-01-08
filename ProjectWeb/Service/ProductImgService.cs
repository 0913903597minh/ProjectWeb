using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
   public class ProductImgService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public bool Insert(ProductImg model)
        {
            try
            {
                model.status = 1;
                model.create_date = DateTime.Now;
                db.ProductImgs.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(ProductImg model)
        {
            try
            {
                ProductImg productImg = GetById(model.productimg_id);
                productImg.color = model.color;
                productImg.url = model.url;
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
        public bool Updatestatus(ProductImg model)
        {
            try
            {
                ProductImg productImg = GetById(model.productimg_id);
                productImg.status = model.status;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public ProductImg GetById(int id)
        {
            try
            {
                return db.ProductImgs.Find(id);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public List<ProductImg> GetAll()
        {
            try
            {
                return db.ProductImgs.Where(x=> (x.is_delete != true)).ToList().OrderBy(x => x.productimg_id).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<ProductImg> GetAll(int? projectid)
        {
            try
            {
                return db.ProductImgs.Where(x=>(x.is_delete != true )&&(projectid.HasValue?x.product_id==projectid:true)).OrderBy(x => x.productimg_id).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<ProductImg> GetAll(int skip, int size)
        {
            try
            {
                return db.ProductImgs.Where(x=> (x.is_delete != true)).OrderBy(x =>x.productimg_id).Skip(skip).Take(size).ToList();
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
                return db.ProductImgs.Count(x=> (x.is_delete != true));
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public List<ProductImg> GetAllByProductId(int product_id)
        {
            try
            {
                return db.ProductImgs.Where(x => (x.is_delete != true) && x.product_id.Value == product_id).ToList().OrderByDescending(x => x.productimg_id).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<ProductImg> GetAllByProductId(int product_id,int skip, int size)
        {
            try
            {
                return db.ProductImgs.Where(x=> (x.is_delete != true) && x.product_id.Value==product_id).ToList().OrderByDescending(x => x.productimg_id).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public int CountAllByProductId(int product_id)
        {
            try
            {
                return db.ProductImgs.Count(x => (x.is_delete != true) && x.product_id.Value == product_id);
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public bool CheckColor(ProductImg model)
        {
            try
            {
                var product = db.ProductImgs.FirstOrDefault(x => (x.is_delete == false)
                && x.color.Trim().ToLower() == model.color.Trim().ToLower() && x.productimg_id!=model.productimg_id  && x.product_id == model.product_id);
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
