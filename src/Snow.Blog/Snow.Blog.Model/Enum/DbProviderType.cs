using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Blog.Model.Enum
{
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DbProviderType : byte
    {
        /// <summary>
        /// 微软 SqlServer 数据库
        /// </summary>
        SqlServer,

        /// <summary>
        /// 开源 MySql数据库
        /// </summary>
        MySql,

        /// <summary>
        /// 嵌入式轻型数据库 SQLite
        /// </summary>
        SQLite,

        /// <summary>
        /// 甲骨文 Oracle
        /// </summary>
        Oracle,

        /// <summary>
        /// 开放数据库互连
        /// </summary>
        ODBC,

        /// <summary>
        /// 面向不同的数据源的低级应用程序接口
        /// </summary>
        OleDb,

        /// <summary>
        /// 微软文档型数据库
        /// </summary>
        Access,

        /// <summary>
        /// 跨平台的关系数据库系统 Firebird
        /// </summary>
        Firebird,

        /// <summary>
        ///加州大学伯克利分校计算机系开发的关系型数据库 PostgreSql
        /// </summary>
        PostgreSql,

        /// <summary>
        /// IBM出口的一系列关系型数据库管理系统 DB2
        /// </summary>
        DB2,

        /// <summary>
        /// IBM公司出品的关系数据库管理系统（RDBMS）家族 Informix
        /// </summary>
        Informix,

        /// <summary>
        /// 微软推出的一个适用于嵌入到移动应用的精简数据库产品 SqlServerCe
        /// </summary>
        SqlServerCe
    }
}