using Cl.Blog.BLL;
using Cl.Blog.Model;
using Cl.Blog.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cl.Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private int PageIndex = Convert.ToInt32(ConfigurationManager.AppSettings["pageIndex"]);
        private readonly int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);

        private readonly BloggerBll CurrentBloggerBll = null;
        private readonly CategoryBll CurrentCategoryBll = null;
        public HomeController()
        {
            CurrentBloggerBll = new BloggerBll();
            CurrentCategoryBll = new CategoryBll();
        }

        // GET: Home
        public ActionResult Index(int categoryId = -1)
        {
            var categories = CurrentCategoryBll.Load<Category>().Select(c => new { Id = c.Id, Name = c.Name })
                .ToDictionary(key => key.Id, value => value.Name);
            ViewData["category"] = categories;
            ViewData["data"] = JsonConvert.SerializeObject(List());
            return View();
        }
        private object List(int categoryId = -1)
        {
            var categories = CurrentCategoryBll.Load<Category>().Select(c => new { Id = c.Id, Name = c.Name })
               .ToDictionary(key => key.Id, value => value.Name);
            int pageCount, totalCount;
            var bloggers = CurrentBloggerBll.LoadPage<Blogger>(
                new PageInfo("*", "ID,Title,CreateDate,Description,CategoryId", "Blogger", "Id", "", "Sort DESC", PageSize, PageIndex),
                out pageCount, out totalCount, categoryId: categoryId);
            return new
            {
                list = from b in bloggers
                       join c in categories on b.CategoryId equals c.Key
                       select new
                       {
                           b.Id,
                           b.Title,
                           b.Description,
                           CreateDate = b.CreateDate.HasValue ? b.CreateDate.Value.ToString("yyyy-MM-DD HH:mm:ss") : "1999-01-01 00:00:00",
                           CategoryName = c.Value
                       },
                pageIndex = PageIndex,
                pageCount = pageCount,
                categoryId = categoryId,
                totalCount = totalCount
            };
        }

        public ActionResult List(int categoryId, int pageIndex)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            return Json(List(categoryId));
        }
    }
}