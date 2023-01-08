using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
   public class WarehouseService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public bool Insert(Warehouse model)
        {
            try
            {
                model.sold = 0;
                model.discount = 0;
                model.amount = 0;
                model.status = 1;
                model.create_date = DateTime.Now;
                db.Warehouses.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateAmount(Warehouse model)
        {
            try
            {
                Warehouse warehouse = GetById(model.id);
                warehouse.amount = model.amount;
                warehouse.update_date = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(Warehouse model)
        {
            try
            {
                Warehouse warehouse = GetById(model.id);
                warehouse.Size = model.Size;
                warehouse.amount = model.amount;
                warehouse.discount = model.discount;
                warehouse.update_date = DateTime.Now;
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
                Warehouse warehouse = GetById(id);
                warehouse.is_delete = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public List<Warehouse> GetAll(int skip, int size)
        {
            try
            {
                return db.Warehouses.Where(x => x.is_delete == false || x.is_delete == null).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
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
                return db.Warehouses.Count(x => x.is_delete == false || x.is_delete == null);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Warehouse> GetAllByKey(int productImg,int skip, int size)
        {
            try
            {
                return db.Warehouses.Where(x => (x.is_delete == false || x.is_delete == null)&&x.productimg_id==productImg).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Warehouse> GetAllByKey(int productImg)
        {
            try
            {
                return db.Warehouses.Where(x => (x.is_delete == false || x.is_delete == null) && x.productimg_id == productImg).OrderBy(x => x.size_id).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int CountAllBykey(int productImg)
        {
            try
            {
                return db.Warehouses.Count(x => (x.is_delete == false || x.is_delete == null) && x.productimg_id == productImg);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public Warehouse GetById(int id)
        {
            try
            {
                return db.Warehouses.FirstOrDefault(x => x.id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Warehouse> GetAll()
        {
            try
            {
                return db.Warehouses.Where(x => x.is_delete == false || x.is_delete == null).ToList().OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public bool CheckSize(Warehouse model){
            try
            {
                int count =  db.Warehouses.Count(x => (x.is_delete == false || x.is_delete == null)&& x.size_id ==model.size_id && x.productimg_id ==model.productimg_id);
                if (count > 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }
        public Warehouse GetBySize( int ProductImgId,int SizeId)
        {
            try
            {
                return db.Warehouses.FirstOrDefault(x => (x.is_delete == false || x.is_delete == null) && x.size_id == SizeId && x.productimg_id == ProductImgId);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public void UpdateCode()
        {
            db.Warehouses.ToList().ForEach(x => x.code = x.ProductImg.product_id.Value.ToString() + x.productimg_id.Value.ToString() + x.size_id.Value.ToString());
            db.SaveChanges();
        }
    }
}
