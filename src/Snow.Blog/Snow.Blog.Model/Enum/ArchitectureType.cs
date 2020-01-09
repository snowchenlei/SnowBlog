using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Blog.Model.Enum
{
    /// <summary>
    /// 数据源架构信息类型 <seealso
    /// cref="http://www.cnblogs.com/jinzhao/archive/2009/07/29/1534023.html"/><seealso cref="https://msdn.microsoft.com/zh-cn/library/system.data.sqlclient.sqlclientmetadatacollectionnames.aspx"/>
    /// </summary>
    public enum ArchitectureType
    {
        [Description("列集合")]
        /// <summary>
        /// 列集合
        /// </summary>
        Columns,

        [Description("数据库集合")]
        /// <summary>
        /// 数据库集合
        /// </summary>
        Databases,

        [Description("")]
        /// <summary>
        /// </summary>
        ForeignKeys,

        [Description("")]
        /// <summary>
        /// </summary>
        IndexColumns,

        [Description("索引集合")]
        /// <summary>
        /// 索引集合
        /// </summary>
        Indexes,

        [Description("参数集合")]
        /// <summary>
        /// 参数集合
        /// </summary>
        Parameters,

        [Description("")]
        /// <summary>
        /// </summary>
        ProcedureColumns,

        [Description("存储过程集合")]
        /// <summary>
        /// 存储过程集合
        /// </summary>
        Procedures,

        [Description("表集合")]
        /// <summary>
        /// 表集合
        /// </summary>
        Tables,

        [Description("")]
        /// <summary>
        /// </summary>
        UserDefinedTypes,

        [Description("用户集合")]
        /// <summary>
        /// 用户集合
        /// </summary>
        Users,

        [Description("")]
        /// <summary>
        /// </summary>
        ViewColumns,

        [Description("视图集合")]
        /// <summary>
        /// 视图集合
        /// </summary>
        Views
    }
}