using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class SlideService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public List<Slide> GetAll()
        {
            try
            {
                return db.Slides.Where(x=>x.is_delete == false||x.is_delete == null).ToList();
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
                return db.Slides.Count(x => x.is_delete == false || x.is_delete == null);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Slide> GetAllByStatus(int status)
        {
            try
            {
                return db.Slides.Where(x => x.is_delete != true && (status != -1 ? x.slide_status == status : true)).ToList().OrderBy(x => x.slide_id).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Slide> GetAllByStatus(int status,int skip, int size)
        {
            try
            {
               
               return db.Slides.Where(x =>x.is_delete != true  && (status != -1 ? x.slide_status == status : true)).ToList().OrderBy(x=>x.slide_id).Skip(skip).Take(size).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int CountAllByStatus(int status)
        {
            try
            {
                return db.Slides.Count(x => (x.is_delete == false || x.is_delete == null) && (status != -1 ? x.slide_status == status : true));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public Slide GetById(int id)
        {
            try
            {
                return db.Slides.FirstOrDefault(x => x.slide_id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public bool Insert(Slide model)
        {
            try
            {
                db.Slides.Add(model);
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
        public bool Delete(Slide slide)
        {
            try
            {
                db.Slides.Remove(slide);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
