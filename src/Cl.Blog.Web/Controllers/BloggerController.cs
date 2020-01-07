using Cl.Blog.BLL;
using Cl.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cl.Blog.Web.Controllers
{
    public class BloggerController : Controller
    {
        // GET: Blogger
        private BloggerInfoBll CurrentBloggerBll = null;
        public BloggerController()
        {
            CurrentBloggerBll = new BloggerInfoBll();
        }
        public ActionResult Index(int id = 1)
        {
            if (id <= 0)
            {
                return Redirect("/Home/Index");
            }
            BloggerInfo blogger = blogger = CurrentBloggerBll.FirstOrDefault<BloggerInfo>(id);
            if(blogger == null)
            {
                return Redirect("/Home/Index");
            }
            return View(blogger);
        }
    }
    
}