using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using static Dapper.SqlBuilder;

namespace Snow.Blog.DAL
{
    public class BaseDal<TEntity> : BaseDal<TEntity, int> where TEntity : class, new()
    {
        public BaseDal(string connectionName = "ConnectionString") : base(connectionName)
        {
        }
    }

    /// <summary>
    /// 通用仓储
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    public class BaseDal<TEntity, TPrimaryKey> where TEntity : class, new()
    {
        /// <summary>
        /// 连接字符串名
        /// </summary>
        protected readonly string ConnectionName;

        /// <summary>
        /// 表名
        /// </summary>
        protected readonly string TableName;

        public BaseDal(string connectionName = "ConnectionString")
        {
            TableName = typeof(TEntity).Name;
            ConnectionName = connectionName;
        }

        #region 查询系

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="wheres">过滤条件</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">sql类型</param>
        /// <returns>是否存在</returns>
        public bool IsExists(Dictionary<string, object> wheres, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            string sql = $"SELECT ISNULL((SELECT TOP(1) 1 FROM {TableName} /**where**/), 0)";
            SqlBuilder builder = new SqlBuilder();
            Template template = builder.AddTemplate(sql);
            foreach (KeyValuePair<string, object> item in wheres)
            {
                builder.Where(item.Key, item.Value);
            }
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                return IsExists(template.RawSql, template.Parameters, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereSql">过滤条件</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">sql类型</param>
        /// <returns>是否存在</returns>
        public bool IsExists(string whereSql, object para, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            string sql = $"SELECT ISNULL((SELECT TOP(1) 1 FROM {TableName} WHERE 1=1 {whereSql}), 0)";
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                return connection.ExecuteScalar<bool>(sql, para, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 获取首行数据
        /// </summary>
        /// <param name="whereSql">过滤条件</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">sql类型</param>
        /// <param name="columns">要查询的列（*）</param>
        /// <returns>首行数据</returns>
        public TEntity GetFirstOrDefault(string whereSql, object param, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null, string columns = "*")
        {
            string sql = $"SELECT {columns} FROM [{TableName}] WHERE 1=1 {whereSql}";
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                return connection.QueryFirstOrDefault<TEntity>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="wheres">过滤条件</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">sql类型</param>
        /// <param name="select">要查询的列（*）</param>
        /// <returns>满足条件的数据</returns>
        public IEnumerable<TEntity> Get(Dictionary<string, object> wheres,
            IDbTransaction transaction = null, int? commandTimeout = null,
            CommandType? commandType = null, string select = "*")
        {
            string sql = $"SELECT /**select**/ FROM {TableName} /**where**/";
            SqlBuilder builder = new SqlBuilder();
            Template template = builder.AddTemplate(sql);
            builder.Select(select);
            foreach (KeyValuePair<string, object> item in wheres)
            {
                builder.Where(item.Key, item.Value);
            }
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                return connection.Query<TEntity>(template.RawSql, template.Parameters,
                    transaction, true, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="whereSql">过滤条件</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">sql类型</param>
        /// <param name="columns">要查询的列（*）</param>
        /// <returns>满足条件的数据</returns>
        public IEnumerable<TEntity> Get(string whereSql, object param, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null, string columns = "*")
        {
            string sql = $"SELECT {columns} FROM {TableName} WHERE 1=1 {whereSql}";
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                return connection.Query<TEntity>(sql, param, transaction, true, commandTimeout, commandType);
            }
        }

        #region Dapper.Contrib

        /// <summary>
        /// 获取Model-Key为int类型
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>实体</returns>
        public TEntity Get(TPrimaryKey id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                return connection.Get<TEntity>(id, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// 获取Model集合（没有Where条件）
        /// </summary>
        /// <returns>实体集合</returns>
        public IEnumerable<TEntity> GetAll()
        {
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                return connection.GetAll<TEntity>();
            }
        }

        #endregion Dapper.Contrib

        #endregion 查询系

        #region 增删改

        public int Delete(string whereSql, object param, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            string sql = $"DELETE FROM {TableName} WHERE 1=1 {whereSql}";
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionName))
                {
                    return connection.Execute(sql, param, transaction, commandTimeout);
                }
            }
            else
            {
                return connection.Execute(sql, param, transaction, commandTimeout);
            }
        }

        #region Dapper.Contrib

        /// <summary>
        /// 批量插入 Entity
        /// </summary>
        /// <param name="entity">实体集合</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        public long Insert(TEntity entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionName))
                {
                    return connection.Insert(entity, transaction, commandTimeout);
                }
            }
            else
            {
                return connection.Insert(entity, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// 批量插入 Entity
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        public long Insert(IEnumerable<TEntity> entities, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionName))
                {
                    return connection.Insert(entities, transaction, commandTimeout);
                }
            }
            else
            {
                return connection.Insert(entities, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// 批量更新 Entity
        /// </summary>
        /// <param name="entity">实体集合</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>是否成功</returns>
        public bool Update(TEntity entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionName))
                {
                    return connection.Update(entity, transaction, commandTimeout);
                }
            }
            else
            {
                return connection.Update(entity, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// 批量更新 Entity
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>是否成功</returns>
        public bool Update(IEnumerable<TEntity> entities, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionName))
                {
                    return connection.Update(entities, transaction, commandTimeout);
                }
            }
            else
            {
                return connection.Update(entities, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// 批量删除 Entity
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>是否成功</returns>
        public bool Delete(TEntity entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionName))
                {
                    return connection.Delete(entity, transaction, commandTimeout);
                }
            }
            else
            {
                return connection.Delete(entity, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// 批量删除 Entity
        /// </summary>
        /// <param name="entity">实体集合</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>是否成功</returns>
        public bool Delete(IEnumerable<TEntity> entities, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionName))
                {
                    return connection.Delete(entities, transaction, commandTimeout);
                }
            }
            else
            {
                return connection.Delete(entities, transaction, commandTimeout);
            }
        }

        #endregion Dapper.Contrib

        #endregion 增删改

        #region 分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="wheres">过滤条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="asc">正序列</param>
        /// <param name="desc">反序列</param>
        /// <returns>《当页数据, 总记录数》</returns>
        public virtual Tuple<IEnumerable<TEntity>, int> GetPageLoad(Dictionary<string, object> wheres
             , int pageIndex, int pageSize, string[] asc = null, string[] desc = null)
        {
            InitialPageSql(out string countQuery, out string selectQuery);

            SqlBuilder builder = new SqlBuilder();

            var count = builder.AddTemplate(countQuery);
            var selector = builder.AddTemplate(selectQuery, new { PageIndex = pageIndex, PageSize = pageSize });
            if (wheres != null)
            {
                foreach (KeyValuePair<string, object> item in wheres)
                {
                    builder.Where(item.Key, item.Value);
                }
            }

            if (asc != null)
            {
                foreach (string a in asc)
                {
                    if (!string.IsNullOrWhiteSpace(a))
                        builder.OrderBy(a);
                }
            }

            if (desc != null)
            {
                foreach (string d in desc)
                {
                    if (!string.IsNullOrWhiteSpace(d))
                        builder.OrderBy(d + " desc");
                }
            }
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionName))
            {
                var totalCount = connection.QuerySingle<int>(count.RawSql, count.Parameters);
                var rows = QueryPage(selector.RawSql, selector.Parameters, connection);
                return new Tuple<IEnumerable<TEntity>, int>(rows, totalCount);
            }
        }

        protected virtual void InitialPageSql(out string countQuery, out string selectQuery)
        {
            countQuery = $@"SELECT COUNT(1)
                      FROM [{TableName}]
                      /**where**/";

            selectQuery = $@"SELECT *
              FROM  (SELECT ROW_NUMBER() OVER ( /**orderby**/ ) AS RowNum, *
                   FROM   [{TableName}]
                   /**where**/
                  ) AS RowConstrainedResult
              WHERE  RowNum >= ((@PageIndex-1) * @PageSize + 1 )
                AND RowNum <= (@PageIndex) * @PageSize
              ORDER BY RowNum";
        }

        protected virtual IEnumerable<TEntity> QueryPage(string sql, object parameters, IDbConnection connection)
        {
            var res = connection.Query<TEntity>(sql, parameters);
            return res;
        }

        #endregion 分页查询
    }
}