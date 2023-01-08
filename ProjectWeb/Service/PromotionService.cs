using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
  public  class PromotionService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public int GetDiscountByKeyCode(string key)
        {
            try
            {
                return db.Promotions.FirstOrDefault(x => x.prom_keycode == key &&(x.prom_expirationdate.HasValue? x.prom_expirationdate > DateTime.Now:true) && (x.prom_amount.HasValue?x.prom_amount>0:true)).prom_discount.Value;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public Promotion GetByKeyCode(string key)
        {
            try
            {
                return db.Promotions.FirstOrDefault(x => x.prom_keycode == key && (x.prom_expirationdate.HasValue ? x.prom_expirationdate > DateTime.Now : true) && (x.prom_amount.HasValue ? x.prom_amount.Value > 0 : true));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Promotion GetById( int id)
        {
            try
            {
                return db.Promotions.FirstOrDefault(x=>x.prom_id==id);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public bool Insert( Promotion model)
        {
            try
            {
                db.Promotions.Add(model);
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
        public List<Promotion> GetAll()
        {
            try
            {
                return db.Promotions.Where(x => x.is_delete != true).ToList().OrderByDescending(x => x.prom_id).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Promotion> GetAll(int skip,int size)
        {
            try
            {
                return db.Promotions.Where(x => x.is_delete != true).ToList().OrderByDescending(x=>x.prom_id).Skip(skip).Take(size).ToList();
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
                return db.Promotions.Count(x => x.is_delete == false || x.is_delete == null);
            }
            catch (Exception)
            {
                return 0;
            }
        }
       
        public int CountAllByName()
        {
            try
            {
                return db.Promotions.Count(x => x.is_delete == false || x.is_delete == null);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool CheckKeyCode(Promotion model)
        {
            try
            {
                var promotion = db.Promotions.FirstOrDefault(x => (x.is_delete == false || x.is_delete == null) && x.prom_id!=model.prom_id
                && x.prom_keycode.Trim().ToLower()==model.prom_keycode.Trim().ToLower());
                if (promotion != null)
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
