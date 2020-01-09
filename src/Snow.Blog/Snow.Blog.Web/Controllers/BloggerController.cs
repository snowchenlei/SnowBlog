using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Snow.Blog.Web.Controllers
{
    public class BloggerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(int page)
        {
            return PartialView(page);
        }
    }
}