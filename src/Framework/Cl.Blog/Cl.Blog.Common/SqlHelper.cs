using Cl.Blog.Model.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.Common
{
    /// <summary>
    /// 生成Sql语句与构建参数
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        /// 完善sql语句
        /// </summary>
        /// <param name="sqlOperationType">操作的类型</param>
        /// <param name="columns">列名集合</param>
        /// <returns></returns>
        public static string InitSql(DbOperationType dbOperationType, string tableName, List<string> columns = null, string appendSql = "")
        {
            switch (dbOperationType)
            {
                case DbOperationType.Insert:
                    return $"INSERT INTO [{tableName}] ({string.Join(",", columns)}) VALUES (@{string.Join(",@", columns)})";
                case DbOperationType.Update:
                    return $"UPDATE [{tableName}] SET {string.Join(",", columns.Select(c => $"{c} = @{c}"))} {appendSql}";
                case DbOperationType.Delete:
                    return $"DELETE FROM [{tableName}] {appendSql}";
                case DbOperationType.Select:
                    return $"SELECT * FROM [{tableName}] {appendSql}";
                default:
                    throw new ArgumentException("参数有误");
            }
        }

        /// <summary>
        /// 根据对象构建参数化集合并获取操作的列
        /// </summary>
        /// <param name="obj">参数对象</param>
        /// <param name="columns">列集合</param>
        /// <returns>参数集合</returns>
        public static IList<T> GetParameterOutColumns<T>(object obj, out List<string> columns) where T : DbParameter, new()
        {
            if (obj == null)
            {
                throw new ArgumentNullException("参数对象不能为null");
            }
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            IList<T> parameters = new List<T>();
            columns = new List<string>();
            foreach (PropertyInfo item in propertyInfos)
            {
                T parameter = Activator.CreateInstance<T>();
                parameter.ParameterName = "@" + item.Name;
                parameter.Value = item.GetValue(obj) ?? DBNull.Value;
                parameters.Add(parameter);
                columns.Add(item.Name);
            }
            return parameters;
        }

        /// <summary>
        /// 根据对象构建参数化集合
        /// </summary>
        /// <param name="obj">参数对象</param>
        /// <returns>参数集合</returns>
        public static IEnumerable<T> GetParameter<T>(object obj) where T : DbParameter, new()
        {            
            if (obj == null)
            {
                throw new ArgumentNullException("参数对象不能为null");
            }
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            IList<T> parameters = new List<T>();
            foreach (PropertyInfo item in propertyInfos)
            {
                T parameter = Activator.CreateInstance<T>();
                parameter.ParameterName = "@" + item.Name;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = item.GetValue(obj);
                parameters.Add(parameter);

            }
            return parameters;
        }

        /// <summary>
        /// 根据对象构建输出参数集合
        /// </summary>
        /// <param name="patameterNames">参数名数组</param>
        /// <returns>参数集合</returns>
        public static IEnumerable<T> GetOutParameter<T>(IEnumerable<OutParaInfo> outParameterInfo) where T : DbParameter, new()
        {
            if (outParameterInfo.Count() < 0)
            {
                throw new ArgumentException("参数有误");
            }
            List<T> parameters = new List<T>();
            foreach (OutParaInfo paraInfo in outParameterInfo)
            {
                T parameter = Activator.CreateInstance<T>();
                parameter.DbType = paraInfo.DbType;
                parameter.Size = paraInfo.Size;
                parameter.Direction = ParameterDirection.Output;
                parameter.ParameterName = paraInfo.Name;
                parameters.Add(parameter);
            }
            return parameters;
        }

        /// <summary>
        /// DataTable转Insert
        /// </summary>
        /// <typeparam name="T">数据库参数类型</typeparam>
        /// <param name="dt">需要转换的datable</param>
        /// <returns>sql与参数信息</returns>
        public static List<KeyValuePair<string, DbParameter[]>> CreateInsertInfo<T>(DataTable dt) where T : DbParameter, new()
        {
            if (dt == null)
            {
                throw new ArgumentNullException("参数对象不能为null");
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("INSERT INTO [{0}] (", dt.TableName);
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
            sbSql.AppendFormat("{0}) VALUES (@{1})", string.Join(", ", columnNames), string.Join(", @", columnNames));
            string sql = sbSql.ToString();
            List<KeyValuePair<string, DbParameter[]>> sqlInfo = new List<KeyValuePair<string, DbParameter[]>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List<DbParameter> parameters = new List<DbParameter>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    T parameter = Activator.CreateInstance<T>();
                    parameter.ParameterName = "@" + dt.Columns[j].ColumnName;
                    parameter.Value = dt.Rows[i][j].ToString();
                    parameters.Add(parameter);
                }
                sqlInfo.Add(new KeyValuePair<string, DbParameter[]>(sql, parameters.ToArray()));
            }
            return sqlInfo;
        }
    }

    /// <summary>
    /// 输出参数信息
    /// </summary>
    public class OutParaInfo
    {
        public OutParaInfo() { }
        public OutParaInfo(string Name, DbType dbType)
        {
            this.Name = Name;
            this.DbType = dbType;
        }
        public OutParaInfo(string Name,DbType dbType, int size)
        {
            this.Name = Name;
            this.DbType = dbType;
            this.Size = size;
        }
        public string Name { get; set; }
        public DbType DbType { get; set; }
        public int Size { get; set; }
    }
}
