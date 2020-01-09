using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Snow.Blog.Service.Bloggers;
using Snow.Blog.Service.Categories;
using Snow.Blog.Web.Models;
using Snow.Blog.Web.Models.Home;

namespace Snow.Blog.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryBll;
        private readonly IBloggerService _bloggerBll;

        public HomeController(ILogger<HomeController> logger
            , ICategoryService categoryBll
            , IBloggerService bloggerBll)
        {
            _logger = logger;
            this._categoryBll = categoryBll;
            this._bloggerBll = bloggerBll;
        }

        public async Task<IActionResult> Index(int categoryId = -1, int page = 1)
        {
            IndexModel model = new IndexModel();
            var categories = await _categoryBll.GetNavSelectList();
            int count = await _bloggerBll.GetCountAsync();
            model.Categories = categories;
            model.Page = page;
            model.TotalCount = count;
            //ViewData["data"] = JsonConvert.SerializeObject(List());
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}