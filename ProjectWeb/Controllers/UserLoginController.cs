using ProjectWeb.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ProjectWeb.Infrastructure;

namespace ProjectWeb.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin
        public ActionResult Index(string backURL)
        {
            if (Session["User"] != null)
            {
                var user = Session["User"] as User;
                if (user.user_role_id == (int)ProjectWeb.Configuration.UserConfig.Role.ADMIN)
                {
                    return Redirect("/home/index");
                }
            }
            ViewBag.backURL = backURL;
            if (string.IsNullOrEmpty(backURL))
            {
                ViewBag.backURL = "/userlogin/index";
            }
            else if (backURL.Contains("LogOut"))
            {
                ViewBag.backURL = "/home/index";
            }

            Response.AppendHeader("statusCode", "401");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin model, string backURL)
        {
            ViewBag.backURL = backURL;
            if (string.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("UserName", "Tài khoản không được để trống");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu không được rỗng");
            }
            if (ModelState.IsValid)
            {
                ProjectWeb.Service.UserService userService = new ProjectWeb.Service.UserService();
                var user = userService.Login(model.UserName, ProjectWeb.Utilities.SecurityHelper.sha256Hash(model.Password));
                if (user != null)
                {
                    Session["User"] = user;
                    //string str = user.AccountOpenFire + "," + user.PasswordOpenFire + "," + user.DomainOpenFire;
                    //Session["KeyOpenFire"] = LibData.Utilities.SecurityHelper.Encrypt(str);
                    if (string.IsNullOrEmpty(backURL))
                    {
                        //return RedirectToAction("/home/index");
                        return Redirect("/home/index");
                    }
                    else
                    {
                        return Redirect(backURL);
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Tài khoản mật khẩu không đúng xin vui lòng thử lại.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(UserSignup model, string backURL)
        {
            ViewBag.backURL = backURL;
            if (string.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("UserName", "Tài khoản không được để trống");
            }
            if (string.IsNullOrEmpty(model.UserEmail))
            {
                ModelState.AddModelError("UserEmail", "Email không được để trống");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu không được rỗng");
            }
            if (ModelState.IsValid)
            {
                ProjectWeb.Service.UserService userService = new ProjectWeb.Service.UserService();
                var user = userService.Login(model.UserName, ProjectWeb.Utilities.SecurityHelper.sha256Hash(model.Password));
                if (user != null)
                {
                    Session["User"] = user;

                    if (string.IsNullOrEmpty(backURL))
                    {
                        //return RedirectToAction("/home/index");
                        return Redirect("/home/index");
                    }
                    else
                    {
                        return Redirect(backURL);
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Tài khoản mật khẩu không đúng xin vui lòng thử lại.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }


        //Logout
        public ActionResult LogOut()
        {
            Session["User"] = null;
            return RedirectToAction("Index");
        }
    }
}