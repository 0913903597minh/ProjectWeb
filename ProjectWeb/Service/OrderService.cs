using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class OrderService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public bool Insert(Order model)
        {
            try
            {
                db.Orders.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(Order model)
        {
            try
            {
                Order order = GetById(model.order_id);
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
                Order order = GetById(id);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Order GetById(int id)
        {
            try
            {
                return db.Orders.Find(id);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public List<Order> GetAll()
        {
            try
            {
                return db.Orders.ToList().OrderByDescending(x => x.order_createdate).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<Order> GetAll(int skip,int size)
        {
            try
            {
                return db.Orders.ToList().OrderByDescending(x => x.order_createdate).Skip(skip).Take(size).ToList();
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
                return db.Orders.Count();
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public List<Order> GetAllByKey(string key,int status,int type,int paid,int skip, int size)
        {
            try
            {
                return db.Orders.Where(x =>(paid != -1 ? (paid == (int)Configuration.OrderConfig.Pay.PAID 
                ? (x.order_status == (int)Configuration.OrderConfig.Status.FINISH || x.customer_pay >= x.total ) 
                : (x.order_status != (int)Configuration.OrderConfig.Status.FINISH && x.customer_pay < x.total))
                :true) 
                && (type != -1 ? x.type == type : true) && (x.buyer_name.Contains(key)||x.phone.Contains(key))
                && (status != -1 ? x.order_status == status : true)).ToList().OrderByDescending(x => x.order_createdate).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int CountAllByKey(string key, int status, int type, int paid)
        {
            try
            {
                return db.Orders.Count(x => (paid != -1 ? (paid == (int)Configuration.OrderConfig.Pay.PAID 
                ? (x.order_status == (int)Configuration.OrderConfig.Status.FINISH || x.customer_pay >= x.total ) 
                : (x.order_status != (int)Configuration.OrderConfig.Status.FINISH && x.customer_pay < x.total)) 
                : true)
                && (type != -1 ? x.type == type : true) && (x.buyer_name.Contains(key) || x.phone.Contains(key))
                && (status != -1 ? x.order_status == status : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public double GetTotalMoney(string key, int status, int type, int paid)
        {
            try
            {
                return db.Orders.Where(x => (paid != -1 ? (paid == (int)Configuration.OrderConfig.Pay.PAID 
                ? x.customer_pay > x.total : x.customer_pay < x.total) 
                : true)
                && (type != -1 ? x.type == type : true) && (x.buyer_name.Contains(key) || x.phone.Contains(key))
                && (status != -1 ? x.order_status == status : true)).Sum(x=>x.total.Value);
            }
            catch (Exception e)
            {
                return 0;
            }
            
        }
        public int CountAllByStatus( int status)
        {
            try
            {
                return db.Orders.Count(x =>(status != -1 ? x.order_status == status : true));
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public bool CheckPhoneUserKey(string phone , string key)
        {
            try
            {
                Order order= db.Orders.FirstOrDefault(x =>  x.keycode.Equals(key) && x.user_phone.Equals(phone));
                if (order != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public List<Order> GetAllByPhone(string phone,int skip,int size)
        {
            try
            {
                return db.Orders.Where(x =>x.phone.Equals(phone) && (x.OrderDetails.Where(m=>m.is_delete == null || m.is_delete.Value == false).Sum(m=>m.amount.Value)>0)).ToList().OrderByDescending(x=>x.order_createdate).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int  CountAllByPhone(string phone)
        {
            try
            {
                return db.Orders.Count(x => x.phone.Equals(phone) && (x.OrderDetails.Where(m=>m.is_delete == null || m.is_delete == false).Sum(m=>m.amount.Value) > 0));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int CountOrderByStatus(int status)
        {
            try
            {
                return db.Orders.Count(x => (x.order_status.Value == status));
            }
            catch (Exception e)
            {
                return 0;
            }
        }

    }
}
