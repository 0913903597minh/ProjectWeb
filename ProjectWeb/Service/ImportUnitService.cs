using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectWeb;

namespace ProjectWeb.Service
{
    public class ImportUnitService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public bool Insert(ImportUnit model)
        {
            try
            {
                model.create_date = DateTime.Now;
                db.ImportUnits.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(ImportUnit model)
        {
            try
            {
                ImportUnit importUnit = GetById(model.id);
                importUnit.name = model.name;
                importUnit.phone = model.phone;
                importUnit.address = model.address;
                //importUnit.IsUpdate= DateTime.Now;
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
                ImportUnit importUnit = GetById(id);
                importUnit.is_delete = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public ImportUnit GetById(int id)
        {
            try
            {
                return db.ImportUnits.Find(id);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public List<ImportUnit> GetAll()
        {
            try
            {
                return db.ImportUnits.Where(x => x.is_delete != true).OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<ImportUnit> GetAll(int skip, int size)
        {
            try
            {
                return db.ImportUnits.Where(x => x.is_delete != true).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
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
                return db.ImportUnits.Count(x => x.is_delete != true);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public bool CheckName(ImportUnit model)
        {
            try
            {
                var importUnit= db.ImportUnits.FirstOrDefault(x => (x.is_delete != true)&&x.id!=model.id
                &&x.name.Trim().ToLower()==model.name.Trim().ToLower());
                if (importUnit != null)
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
