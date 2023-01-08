using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectWeb.Areas.Admin.Models;
using ProjectWeb.Infrastructure;

namespace ProjectWeb.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string backURL)
        {         
            if (Session["User"] != null)
            {
                var user = Session["User"] as User;
                if (user.user_role_id == (int)ProjectWeb.Configuration.UserConfig.Role.ADMIN)
                {
                    return Redirect("/admin/dashboard");
                }
            }

            ViewBag.backURL = backURL;
            if (string.IsNullOrEmpty(backURL))
            {
                ViewBag.backURL = "/admin/dashboard";
            }
            else if (backURL.Contains("LogOut"))
            {
                ViewBag.backURL = "/admin/dashboard/index";
            }

            Response.AppendHeader("statusCode", "401");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLogin model, string backURL)
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
                if (user != null && user.user_role_id == 1)
                {
                    Session["User"] = user;
                    //string str = user.AccountOpenFire + "," + user.PasswordOpenFire + "," + user.DomainOpenFire;
                    //Session["KeyOpenFire"] = ProjectWeb.Utilities.SecurityHelper.Encrypt(str);
                    if (string.IsNullOrEmpty(backURL))
                    {
                        return RedirectToAction("/admin/dashboard");
                    }
                    else
                    {
                        return Redirect(backURL);
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Tài khoản hoặc mật khẩu không đúng. Xin vui lòng thử lại!");
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

        //ChangePass
        [CustomAuthenticationFilter]
        [CustomAuthorize("Admin")]
        [HttpGet]
        public ActionResult ChangePass()
        {
            return View(new ProjectWeb.Models.ChangePass());
        }
        [CustomAuthenticationFilter]
        [CustomAuthorize("Admin")]
        [HttpPost]
        public ActionResult ChangePass(ProjectWeb.Models.ChangePass model)
        {
            ProjectWeb.Service.UserService userService = new ProjectWeb.Service.UserService();
            ProjectWeb.User user = Session["User"] as ProjectWeb.User;
            if (ProjectWeb.Utilities.SecurityHelper.sha256Hash(model.oldPass) != user.user_password.Trim())
            {
                ModelState.AddModelError("oldPass", "Mật khẩu cũ sai");
                model.oldPass = "";
            }
            if (string.IsNullOrEmpty(model.oldPass))
            {
                ModelState.AddModelError("oldPass", "Mật khẩu cũ không được để trống");
            }
            if (!model.newPass.Equals(model.renewPass))
            {
                ModelState.AddModelError("newPass", "Mật khẩu mới và mât khẩu nhập lại không trùng");
            }
            if (string.IsNullOrEmpty(model.newPass))
            {
                ModelState.AddModelError("newPass", "Mật khẩu mới không được để trống");
                if (string.IsNullOrEmpty(model.renewPass))
                {
                    ModelState.AddModelError("renewPass", "Mật khẩu nhập lại không được để trống");
                }
            }
            if (!string.IsNullOrEmpty(model.newPass))
            {
                if (model.newPass.Length < 6)
                {
                    ModelState.AddModelError("newPass", "Mật khẩu mới phải ít nhất 6 ký tự");
                }
            } 
            if (ModelState.IsValid)
            {
                user.user_password = ProjectWeb.Utilities.SecurityHelper.sha256Hash(model.newPass);
                if (userService.UpdatePassword(user))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return View(model);
                }
            }
            return View(model);
        }

    }
}