using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.Common
{
    public class ModelToSql
    {
        public string GetSql<T>(OperationType operationType, DbType dbType) where T : class
        {
            switch (operationType)
            {
                case OperationType.INSERT:
                    break;
                case OperationType.DELETE:
                    break;
                case OperationType.UPDATE:
                    break;
                default:
                case OperationType.SELECT:
                    break;
            }
        }
        public string InsertSql<T>(DbType dbType)where T:class
        {
            string sql = string.Empty;
            switch (dbType)
            {
                case DbType.SqlServer:
                    sql = $"INSERT INTO {typeof(T).ToString()} () VALUES ()";
                    break;
                case DbType.MySql:
                    break;
                case DbType.Oracle:
                    break;
                case DbType.Sqlite:
                    break;
                default:
                    break;
            }
        }
    }
    public enum OperationType
    {
        INSERT,
        DELETE,
        UPDATE,
        SELECT
    }
    public enum DbType
    {
        SqlServer,
        MySql,
        Oracle,
        Sqlite
    }
}
