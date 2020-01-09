using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Blog.Model.Enum
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public enum SourceType
    {
        /// <summary>
        /// 原创
        /// </summary>
        [Description("原创")]
        creator = 1,

        /// <summary>
        /// 转载
        /// </summary>
        [Description("转载")]
        reprinted,

        /// <summary>
        /// 翻译
        /// </summary>
        [Description("翻译")]
        translate
    }
}