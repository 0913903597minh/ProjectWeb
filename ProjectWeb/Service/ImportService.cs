using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class ImportService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public bool Insert(Import model)
        {
            try
            {
                    db.Imports.Add(model);
                    db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(Import model)
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
                Import import = GetById(id);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Import GetById(int id)
        {
            try
            {
                return db.Imports.Find(id);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public List<Import> GetAll()
        {
            try
            {
                return db.Imports.OrderByDescending(x => x.create_date).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<Import> GetAll(int skip, int size)
        {
            try
            {
                return db.Imports.OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
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
                return db.Imports.Count();
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public List<Import> GetAllByKey(int importunitid,int skip, int size)
        {
            try
            {
                return db.Imports.Where(x => (importunitid>0 ? x.importunit_id == importunitid : true)).ToList().OrderByDescending(x => x.create_date).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public int CountAllByKey( int importunitid)
        {
            try
            {
                return db.Imports.Count(x => (importunitid>0? x.importunit_id == importunitid : true));
            }
            catch (Exception e)
            {
                return 0;
            }

        }
    }
}
