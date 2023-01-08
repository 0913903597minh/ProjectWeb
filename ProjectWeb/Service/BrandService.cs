using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class BrandService
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public bool Insert(Brand model)
        {
            try
            {
                model.create_date = DateTime.Now;
                db.Brands.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(Brand model)
        {
            try
            {
                Brand brand = GetById(model.brand_id);
                brand.brand_name = model.brand_name;
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
        public bool Delete(int id)
        {
            try
            {
                Brand brand = GetById(id);
                brand.is_delete = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Brand GetById(int id)
        {
            try
            {
                return db.Brands.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Brand> GetAll()
        {
            try
            {
                return db.Brands.Where(x => x.is_delete != true).ToList().OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<Brand> GetAll(int skip, int size)
        {
            try
            {
                var list = db.Brands.Where(x => x.is_delete != true).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size);
                return list.ToList();
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
                return db.Brands.Count(x => x.is_delete == false || x.is_delete == null);
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public bool CheckName(Brand model)
        {
            try
            {
                var brand = db.Brands.FirstOrDefault(x => (x.is_delete == false || x.is_delete == null) && x.brand_id != model.brand_id
                && x.brand_name.Trim().ToLower() == model.brand_name.Trim().ToLower());
                if (brand != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
