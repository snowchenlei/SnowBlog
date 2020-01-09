using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snow.Blog.Model.Enum;

namespace Snow.Blog.Model.DataBase
{
    /// <summary>
    /// 博客视图模型
    /// </summary>
    public class BloggerInfo
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType SourceType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public List<Tag> Tag { get; set; }

        /// <summary>
        /// 纯文本内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 编码后的Html
        /// </summary>
        public string HtmlEncoded { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int? ViewCount { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 是否展示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? EditDate { get; set; }
    }
}