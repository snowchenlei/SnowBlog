using Cl.Blog.BLL;
using Cl.Blog.Model;
using Cl.Blog.Web.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cl.Blog.Web.Areas.Manager.Controllers
{
    public class LoginController : Controller
    {
        AdminBll AdminBll;
        public LoginController()
        {
            AdminBll = new AdminBll();
        }
        // GET: Manager/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if(string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.PassWord))
            {
                return Content("<script>alert('用户名或密码不能为空')</script>");
            }
            Admin admin = AdminBll.Login(login.UserName, login.PassWord);
            if(admin == null)
            {
                return Content("<script>alert('用户名或密码不正确')</script>");
            }
            Session["User"] = admin;
            return Redirect("/Home");
        }
    }
}