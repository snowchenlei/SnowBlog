using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Blog.Service.Bloggers.Dto
{
    public class BloggerListDto
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public string CategoryName { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public string LastModifyDate { get; set; }
    }
}