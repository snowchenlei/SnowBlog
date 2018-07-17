using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cl.Blog.Web.Areas.Manager.Models
{
    /// <summary>
    /// 分类操作
    /// </summary>
    public class VCategory
    {
        /// <summary>
        /// 父分类Id
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否在导航菜单显示
        /// </summary>
        public bool IsNavShow { get; set; }
    }
}