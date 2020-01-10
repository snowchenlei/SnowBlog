using System.Collections.Generic;

namespace Snow.Blog.Service.Dto
{
    public class PagedResultDto<T> : ListResultDto<T>
    {
        public int TotalCount { get; set; }
        //public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount => TotalCount / Limit + (TotalCount % Limit > 0 ? 1 : 0);

        /// <summary>
        /// 是否有前页
        /// </summary>
        public bool HasPrevious => Page > 0 && PageCount > 0;

        /// <summary>
        /// 是否有后页
        /// </summary>
        public bool HasNext => Page < PageCount - 1;

        public PagedResultDto()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int page, int limit, int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
            Page = page;
            Limit = limit;
        }
    }
}