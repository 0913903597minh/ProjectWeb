using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Infrastructure;
using ProjectWeb.Service;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]

    [CustomAuthorize("Admin")]
    public class SlideController : Controller
    {
        public const string _ImagesPath = "~/Images/Slider";
        // GET: Slide
        public ActionResult Index(int status = -1  ,int page = 1, int size = 5)
        {
            ViewBag.page = page;
            ViewBag.status = status;
            ViewBag.size = size;
            return View();
        }
        public ActionResult ListSlide(int status = -1 ,int page = 1, int size = 5)
        {
            ViewBag.status = status;
            ViewBag.page = page;
            ViewBag.size = size;
            int skip = (page - 1) * size;
            SlideService slideService = new SlideService();
            var list = slideService.GetAllByStatus(status, skip, size);
            var count = slideService.CountAllByStatus(status);
            StaticPagedList<Slide> pagedList = new StaticPagedList<Slide>(list, page, size, count);
            return View(pagedList);

        }
        [HttpGet]
        public ActionResult AddSlide(int ?id)
        {
            Slide slide = new Slide();
            if (id.HasValue)
            {
                slide = new SlideService().GetById(id.Value);
            }
            return View(slide);
        }
        [HttpPost]
        public ActionResult AddSlide(Slide model, HttpPostedFileBase slide_urlfile)
        {
            SlideService slideService = new SlideService();
            if (model.slide_id == 0 && slide_urlfile == null)
            {
                ModelState.AddModelError("UrlFlie", "Thêm ảnh");
            }
            if (slide_urlfile != null)
            {
                try
                {
                    string fileName = Guid.NewGuid() + Path.GetFileName(slide_urlfile.FileName);
                    string path = Path.Combine(Server.MapPath(_ImagesPath), fileName);
                    slide_urlfile.SaveAs(path);
                    model.slide_urlfile = fileName;
                }
                catch (Exception)
                {

                    ModelState.AddModelError("UrlFlie", "Ảnh bị lỗi");
                }
           
            }
            if (ModelState.IsValid)
            {
               
                if (model.slide_id > 0)
                {
                    var old = slideService.GetById(model.slide_id);
                  
                    old.slide_status = model.slide_status;
                    if (slide_urlfile != null)
                    {
                     
                        old.slide_urlfile = model.slide_urlfile;
                    }
                    
                    if (slideService.Update())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return View("AddSlide", model);
                    }
                }
                else
                {
                    model.slide_name = "SLIDE" + DateTime.Now.ToString("yyyyMMddHHmmss");
                  
                    if (slideService.Insert(model))
                    {
                        model.ordernumber = model.slide_id;
                        if (slideService.Update())
                        {
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return View("AddSlide", model);
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ChangeOrder(int id)
        {
            ViewBag.ListSlide = new SlideService().GetAllByStatus((int)Configuration.SlideConfig.Status.SHOW);
            ViewBag.id = id;
            ViewBag.replaceorderid = 0;
            return View();
        }
        [HttpPost]
        public ActionResult ChangeOrder(int id, int replaceorderid)
        {
            ViewBag.id = id;
            ViewBag.replaceorderid = replaceorderid;
            SlideService slideService = new SlideService();
            ViewBag.ListSlide = slideService.GetAllByStatus((int)Configuration.SlideConfig.Status.SHOW);
            Slide slide = slideService.GetById(id);
            Slide slide1 = slideService.GetById(replaceorderid);
            if (slide == null)
            {
                ModelState.AddModelError("id", "Lỗi");
            }
            if (slide1 == null)
            {
                ModelState.AddModelError("replaceorderid", "Lỗi");
            }
            if (ModelState.IsValid)
            {
                int temp= slide.ordernumber ?? 0;
                slide.ordernumber = slide1.ordernumber;
                slide1.ordernumber = temp;
              
                if (slideService.Update())
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View();
                }
                ModelState.AddModelError("error", "Lỗi");
                return View();
            }
            return View();
        }
        public bool Delete(int id)
        {
            SlideService slideService = new SlideService();
            Slide slide = slideService.GetById(id);
            if (slide != null)
            {
                if (slideService.Delete(slide))
                {
                    return true;
                }
            }
            return false;
        }
    }
}