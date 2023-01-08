using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectWeb;

namespace ProjectWeb.Service
{
    public class CartService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public Cart GetByProductAndKey(int wareId, int cookieId)
        {
            try
            {
                return db.Carts.FirstOrDefault(x => x.warehouse_id == wareId && x.status == 1);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Cart GetByProductAndKey(int id, string key)
        {
            try
            {
                return db.Carts.FirstOrDefault(x => x.warehouse_id == id && x.status == 1 );
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Cart GetById(int Id)
        {
            try
            {
                return db.Carts.FirstOrDefault(x => x.cart_id == Id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Cart> GetAllByKey(string key)
        {
            try
            {
                return db.Carts.Where(x => x.status == Configuration.CartConfig.INCART).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool Insert(Cart cart)
        {
            try
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Update(Cart cart)
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public double GetAmount(int wareid)
        {
            try
            {
                return db.Carts.Where(x => x.warehouse_id.Value == wareid && x.status.Value == 1).Sum(x => x.amount.Value);
            }
            catch (Exception e)
            {
                return 0;
            }
           
        }
        public bool Remove(Cart cart)
        {
            try
            {
                db.Carts.Remove(cart);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveProductInCart(int id)
        {
            try
            {
                Cart cart = GetById(id);
                db.Carts.Remove(cart);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
