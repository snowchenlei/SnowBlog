using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.Model
{
    /// <summary>
    /// 分页相关信息
    /// </summary>
    public struct PageInfo
    {
        public PageInfo(string fieldSql, string field, string tableName, string primaryKey, string whereSql, string orderSql, int pageSize, int pageIndex) : this()
        {
            this.FieldSql = fieldSql;
            this.Field = field;
            this.TableName = tableName;
            this.PrimaryKey = primaryKey;
            this.WhereSql = whereSql;
            this.OrderSql = orderSql;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }
        public string FieldSql { get; set; }
        public string Field { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public string WhereSql { get; set; }
        public string OrderSql { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
