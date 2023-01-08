using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class OrderDetailService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public int GetAmountWait(int id)
        {
            try
            {
                return db.OrderDetails.Where(x => x.warehouse_id == id && (x.is_delete == null || x.is_delete == false) && x.Order.order_status == (int)Configuration.OrderConfig.Status.WAIT).Sum(x => x.amount.Value);
                //return db.OrderDetails.Where(x => x.WarehouseId == id && (x.is_delete == null || x.is_delete == false) && x.Order.order_status == (int)Configuration.OrderConfig.Status.WAIT).Sum(x => x.Amount.Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool InsertAll(List<OrderDetail> list)
        {
            try
            {
                foreach (var item in list)
                {
                    db.OrderDetails.Add(item);
                }
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
