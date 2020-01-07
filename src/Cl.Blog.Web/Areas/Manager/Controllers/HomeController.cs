using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cl.Blog.Web.Areas.Manager.Controllers
{
    public class HomeController : Controller
    {
        //GET: Manager/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}