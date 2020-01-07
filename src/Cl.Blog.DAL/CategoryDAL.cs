using Cl.Blog.Common;
using Cl.Blog.Model;
using Cl.Blog.Model.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.DAL
{
    public class CategoryDal:BaseDal
    {

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="columnObj">基本的sql条件</param>
        /// <param name="whereObj">过滤sql条件</param>
        /// <returns>受影响的行数</returns>
        public int Add(Category category)
        {
            //string sql = "INSERT INTO Category(PID,Name,Alias,Remark,Sort,IsDelete,IsNavShow,CreateDate) VALUES (@PID,@Name,@Alias,@Remark,@Sort,@IsDelete,@IsNavShow,@CreateDate)";
            var paraObj = new
            {
                PID = category.PId,
                Name = category.Name,
                Alias = category.Alias,
                Remark = category.Remark,
                Sort = category.Sort,
                IsDelete = false,
                IsNavShow = category.IsNavShow,
                CreateDate = DateTime.Now
            };
            List<string> columns = null;
            var parameters = SqlHelper.GetParameterOutColumns<SqlParameter>(paraObj, out columns);
            string sql = SqlHelper.InitSql(DbOperationType.Insert, "Category", columns);
            return CurrentDbOperation.ExecuteNonQuery(sql, parameters.ToArray());
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="columnObj">列名对象</param>
        /// <param name="appendSql">附加sql语句</param>
        /// <param name="whereObj">过滤sql条件</param>
        /// <returns>受影响的行数</returns>
        public int Edit(Category category)
        {
            var paraObj = new
            {
                PID = category.PId,
                Name = category.Name,
                Alias = category.Alias,
                Remark = category.Remark,
                Sort = category.Sort,
                IsDelete = false,
                IsNavShow = category.IsNavShow,
                CreateDate = DateTime.Now
            };

            List<string> columns = null;
            var parameters = SqlHelper.GetParameterOutColumns<SqlParameter>(paraObj, out columns);
            parameters.Add(new SqlParameter("@Id", category.Id));
            string sql = SqlHelper.InitSql(DbOperationType.Update, "Category", columns, " WHERE Id=@Id");
            return CurrentDbOperation.ExecuteNonQuery(sql, parameters.ToArray());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>状态信息</returns>
        public KeyValuePair<string,string> ProcDelete(int id)
        {
            var outPara = SqlHelper.GetOutParameter<SqlParameter>(
                new OutParaInfo[]
                {
                    new OutParaInfo("@state", DbType.String, 50),
                    new OutParaInfo("@message", DbType.String, 50)
                }).ToList();
            var inPara = SqlHelper.GetParameter<SqlParameter>(new { Id = id, tableName = "Category" });
            var para = outPara.Union(inPara);
            CurrentDbOperation.ExecuteNonQuery("Delete_data", CommandType.StoredProcedure, para.ToArray());
            return new KeyValuePair<string, string>(outPara[0].Value.ToString(), outPara[1].Value.ToString());
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Load(string name, string alias, int pId,DateTime? startTime, DateTime? endTime)
        {
            StringBuilder sbAppendSql = new StringBuilder(" WHERE IsDelete=0 ");
            if(!string.IsNullOrEmpty(name))
            {
                sbAppendSql.AppendFormat(" AND Name LIKE '%{0}%'", name);
            }
            if(!string.IsNullOrEmpty(alias))
            {
                sbAppendSql.AppendFormat(" AND Alias LIKE '%{0}%'", alias);
            }
            if(pId >= 0)
            {
                sbAppendSql.AppendFormat(" AND pID = {0}", pId);
            }
            if (startTime.HasValue)
            {
                sbAppendSql.AppendFormat(" AND CreateDate >= '{0}'", startTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (endTime.HasValue)
            {
                sbAppendSql.AppendFormat(" AND CreateDate <= '{0}'", endTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            sbAppendSql.Append(" ORDER BY sort DESC, CreateDate DESC");
            string sql = SqlHelper.InitSql(DbOperationType.Select, "Category", appendSql: sbAppendSql.ToString());
            return CurrentDbOperation.Query(sql);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Load()
        {
            string sql = SqlHelper.InitSql(DbOperationType.Select, "Category", appendSql: " WHERE IsDelete=0 ORDER BY sort DESC, CreateDate DESC");
            return CurrentDbOperation.Query(sql);
        }

        public DataTable Load(int id)
        {
            string sql = SqlHelper.InitSql(DbOperationType.Select, "Category", appendSql: " WHERE IsDelete=0 AND Id=@Id ORDER BY sort DESC, CreateDate DESC");
            return CurrentDbOperation.Query(sql, SqlHelper.GetParameter<SqlParameter>(new { Id = id }).ToArray());
        }

        /// <summary>
        /// 加载父分类
        /// </summary>
        /// <returns></returns>
        public DataTable LoadParent()
        {
            string sql = "SELECT Id,Name FROM Category WHERE IsDelete=0 AND Pid=0 ORDER BY sort DESC, CreateDate DESC";
            return CurrentDbOperation.Query(sql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns></returns>
        public DataTable Load(int pageSize, int pageIndex, out int pageCount, out int totalCount)
        {
            var outPara = SqlHelper.GetOutParameter<SqlParameter>(
                new OutParaInfo[]
                {
                    new OutParaInfo("PageCount", DbType.Int32),
                    new OutParaInfo("TotalCount", DbType.Int32),
                }).ToList();
            var inPara = SqlHelper.GetParameter<SqlParameter>(new { FieldSql = "*", Field = "ID,PID,Name,Alias,Remark,IsNavShow,Sort,CreateDate,UpdateDate", TableName = "Category", PrimaryKey = "Id", PageIndex = pageIndex, PageSize = pageSize, WhereSql = "IsDelete=0", OrderSql = "Sort DESC" });
            var para = outPara.Union(inPara);
            DataTable result = CurrentDbOperation.Query("P_Pagination", para.ToArray());
            int.TryParse(outPara[0].Value.ToString(), out pageCount);
            int.TryParse(outPara[1].Value.ToString(), out totalCount);
            return result;
        }

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="whereObj">过滤sql条件</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists<T>(string categoryName)
        {
            string sql = "SELECT 1 FROM [Category] WHERE IsDelete=0 AND categoryName=@categoryName";
            return CurrentDbOperation.ExecuteScalar<int>(sql, new SqlParameter("@categoryName", categoryName)) > 0;
        }

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="whereObj">过滤sql条件</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists<T>(int id)
        {
            string sql = "SELECT 1 FROM [Category] WHERE IsDelete=0 AND Id=@Id";
            return CurrentDbOperation.ExecuteScalar<int>(sql, new SqlParameter("@Id", id)) > 0;
        }
    }
}
