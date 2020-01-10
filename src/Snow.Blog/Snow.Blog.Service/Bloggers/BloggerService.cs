using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Snow.Blog.IDAL.Categories;
using Snow.Blog.IRepository.Bloggers;
using Snow.Blog.Model.DataBase;
using Snow.Blog.Service.Bloggers.Dto;
using Snow.Blog.Service.Dto;

namespace Snow.Blog.Service.Bloggers
{
    public class BloggerService : IBloggerService
    {
        private readonly IMapper _mapper;
        private readonly IBloggerRepository _bloggerRepository;

        public BloggerService(
            IMapper mapper,
            IBloggerRepository bloggerRepository)
        {
            this._mapper = mapper;
            this._bloggerRepository = bloggerRepository;
        }

        public async Task<PagedResultDto<BloggerListDto>> GetBloggersPagedAsync(GetBloggerInput input)
        {
            string[] asc = null, desc = null;
            Dictionary<string, object> wheres = new Dictionary<string, object>();
            if (input.CategoryId.HasValue)
            {
                wheres.Add("[CategoryId]=@CategoryId", new
                {
                    CategoryId = input.CategoryId.Value
                });
            }
            switch (input.Order)
            {
                case Model.Enum.OrderType.ASC:
                    asc = input.Sorting.Split(',');
                    break;

                case Model.Enum.OrderType.DESC:
                    desc = input.Sorting.Split(',');
                    break;
            }
            var result = await _bloggerRepository.GetPageLoadAsync(wheres, input.Page, input.Limit, asc, desc);
            return new PagedResultDto<BloggerListDto>(input.Page, input.Limit, result.totalCount, _mapper.Map<List<BloggerListDto>>(result.items));
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<BloggerDetailDto> GetBloggerDetailAsync(int id)
        {
            Blogger blogger = await _bloggerRepository.GetDetailAsync(id);
            return _mapper.Map<BloggerDetailDto>(blogger);
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCountAsync()
        {
            return await _bloggerRepository.GetCountAsync();
        }
    }
}