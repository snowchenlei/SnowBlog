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
    public class AccountController : Controller
    {
        private AdminBll AdminBll;

        public AccountController()
        {
            AdminBll = new AdminBll();
        }

        // GET: Manager/Account
        public ActionResult Login()
        {
            return View();
        }

        public ViewResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResetPassword(string oldPwd, string newPwd, string newQueryPwd)
        {
            if (string.IsNullOrEmpty(oldPwd) || string.IsNullOrEmpty(newPwd) || string.IsNullOrEmpty(newQueryPwd))
            {
                return Json(new { status = 0, message = "密码有误" });
            }
            else if (string.Equals(oldPwd, newPwd))
            {
                return Json(new { status = 0, message = "新旧密码不能相同" });
            }
            if (Session["admin"] == null)
            {
                return Json(new { status = 0, message = "用户" });
            }
            Admin admin = (Admin)Session["admin"];
            if (string.Equals(admin.PassWord, oldPwd))
            {
                return Json(new { status = 0, message = "原密码不正确" });
            }
            AdminBll.ResetPassword(admin.Id, newPwd);
            return Json(new { status = 1 });
        }
    }
}