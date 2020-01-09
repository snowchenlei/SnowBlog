using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snow.Blog.Service.Bloggers.Dto;
using Snow.Blog.Service.Dto;

namespace Snow.Blog.Service.Bloggers
{
    public interface IBloggerService
    {
        Task<PagedResultDto<BloggerListDto>> GetBloggersPagedAsync(GetBloggerInput input);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<BloggerDetailDto> GetBloggerDetailAsync(int id);

        Task<int> GetCountAsync();
    }
}