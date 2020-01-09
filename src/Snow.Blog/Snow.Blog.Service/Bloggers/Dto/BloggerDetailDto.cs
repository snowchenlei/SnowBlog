using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Blog.Service.Bloggers.Dto
{
    public class BloggerDetailDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 编码后的Html
        /// </summary>
        public string HtmlEncoded { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime EditDate { get; set; }
    }
}