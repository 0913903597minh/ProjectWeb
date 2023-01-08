using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class SizeService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public List<Size> GetAll()
        {
            try
            {
                return db.Sizes.OrderBy(x => x.size_id).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Size GetById(int id)
        {
            try
            {
                return db.Sizes.FirstOrDefault(x => x.size_id==id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Size> GetAllBySex(int sex)
        {
            try
            {
                return db.Sizes.Where(x =>(sex!=-1? x.type.Value == sex:true)).OrderBy(x => x.size_id).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
