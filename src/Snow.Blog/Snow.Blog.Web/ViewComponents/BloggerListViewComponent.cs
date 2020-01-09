using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Snow.Blog.Web.ViewComponents
{
    public class BloggerListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int categoryId, int page)
        {
            List<string> result = new List<string>()
            {
                "本地CS的导出xls代码段"+page,
                "本地CS的导出xls代码段"+page,
                "本地CS的导出xls代码段"+page,
            };
            return View(result);
        }
    }
}