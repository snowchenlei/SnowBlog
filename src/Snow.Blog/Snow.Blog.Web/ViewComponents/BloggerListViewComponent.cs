using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Snow.Blog.Service.Bloggers;
using Snow.Blog.Service.Bloggers.Dto;
using Snow.Blog.Service.Dto;

namespace Snow.Blog.Web.ViewComponents
{
    public class BloggerListViewComponent : ViewComponent
    {
        private readonly IBloggerService _bloggerService;

        public BloggerListViewComponent(IBloggerService bloggerService)
        {
            this._bloggerService = bloggerService;
        }

        public async Task<IViewComponentResult> InvokeAsync(GetBloggerInput input)
        {
            PagedResultDto<BloggerListDto> bloggerPage = await _bloggerService.GetBloggersPagedAsync(input);
            IReadOnlyList<BloggerListDto> result = bloggerPage.Items;
            return View(result);
        }
    }
}