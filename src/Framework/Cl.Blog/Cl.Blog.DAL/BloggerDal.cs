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
    public class BloggerDal : BaseDal
    {

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns></returns>
        public DataTable LoadPage(PageInfo pageInfo, out int pageCount, out int totalCount, string title = null, int categoryId = 0, int sourceType = 0, DateTime? startTime = null, DateTime? endTime = null)
        {
            #region where条件拼接
            StringBuilder sbWhereSql = new StringBuilder("IsDelete=0");
            if (!string.IsNullOrEmpty(title))
            {
                sbWhereSql.AppendFormat(" AND Title LIKE '%{0}%'", title);
            }
            if (categoryId > 0)
            {
                sbWhereSql.AppendFormat(" AND CategoryId = '{0}'", categoryId);
            }
            if (sourceType > 0)
            {
                sbWhereSql.AppendFormat(" AND SourceType = '{0}'", sourceType);
            }
            if (startTime.HasValue)
            {
                sbWhereSql.AppendFormat(" AND CreateDate >= '{0}'", startTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (endTime.HasValue)
            {
                sbWhereSql.AppendFormat(" AND CreateDate <= '{0}'", endTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            #endregion
            var outPara = SqlHelper.GetOutParameter<SqlParameter>(
                new OutParaInfo[]
                {
                    new OutParaInfo("PageCount", DbType.Int32),
                    new OutParaInfo("TotalCount", DbType.Int32),
                }).ToList();
            var inPara = SqlHelper.GetParameter<SqlParameter>(new
            {
                FieldSql = "*",
                Field = "ID,SourceType,Title,CategoryId,ViewCount,Sort,Body,Description,IsShow,CreateDate,EditDate",
                TableName = "Blogger",
                PrimaryKey = "Id",
                PageIndex = pageInfo.PageIndex,
                PageSize = pageInfo.PageSize,
                WhereSql = sbWhereSql.Append(pageInfo.WhereSql).ToString(),
                OrderSql = "Sort DESC"
            });
            var para = outPara.Union(inPara);
            DataTable result = CurrentDbOperation.Query("P_Pagination", CommandType.StoredProcedure, para.ToArray());
            int.TryParse(outPara[0].Value.ToString(), out pageCount);
            int.TryParse(outPara[1].Value.ToString(), out totalCount);
            return result;
        }

        public DataTable Load(int id)
        {
            string sql = "SELECT ID,SourceType,Title,CategoryId,ViewCount,Sort,Body,Description,CreateDate,EditDate FROM Blogger";
            return CurrentDbOperation.Query(sql);
        }
        public T LoadFirst<T>(int id) where T : new()
        {
            string sql = "SELECT b.ID,SourceType,Title,CategoryId,b.Sort,HtmlEncoded,Body,Description,b.CreateDate,IsShow,EditDate,c.Name FROM Blogger AS b LEFT JOIN Category AS c ON b.CategoryId = c.ID WHERE b.IsDelete = 0 AND b.ID = @ID";
            return CurrentDbOperation.ExecuteReader<T>(sql, SqlHelper.GetParameter<SqlParameter>(new
            {
                ID = id
            }).ToArray()).FirstOrDefault();
        }
        public DataTable LoadViewFirst<T>(int id) where T : new()
        {
            string sql = "";
            return CurrentDbOperation.Query(sql, SqlHelper.GetParameter<SqlParameter>(new { ID = id }).ToArray());
        }

        /// <summary>
        /// 添加博客
        /// </summary>
        /// <param name="blog">博客信息</param>
        /// <returns>受影响行数</returns>
        public int Add(Blogger blog)
        {
            var paraObj = new
            {
                Title = blog.Title,
                CategoryId = blog.CategoryId,
                SourceType = blog.SourceType,
                //Tag = blog.Tag,
                ViewCount = 0,
                Sort = blog.Sort ?? 0,
                IsShow = blog.IsShow,
                IsDelete = 0,
                HtmlEncoded = blog.HtmlEncoded,
                Body = blog.Body,
                Description = blog.Description,
                CreateDate = DateTime.Now
            };
            List<string> columns = null;
            var parameters = SqlHelper.GetParameterOutColumns<SqlParameter>(paraObj, out columns);
            string sql = SqlHelper.InitSql(DbOperationType.Insert, "Blogger", columns);
            return CurrentDbOperation.ExecuteNonQuery(sql, parameters.ToArray());
        }

        /// <summary>
        /// 修改博客
        /// </summary>
        /// <param name="blog">博客信息</param>
        /// <returns>受影响行数</returns>
        public int Edit(Blogger blog)
        {
            var paraObj = new
            {
                Title = blog.Title,
                CategoryId = blog.CategoryId,
                SourceType = blog.SourceType,
                //Tag = blog.Tag,
                Sort = blog.Sort,
                IsShow = blog.IsShow,
                HtmlEncoded = blog.HtmlEncoded,
                Body = blog.Body,
                Description = blog.Description,
                EditDate = DateTime.Now
            };
            List<string> columns = null;
            var parameters = SqlHelper.GetParameterOutColumns<SqlParameter>(paraObj, out columns);
            parameters.Add(new SqlParameter("@Id", blog.Id));
            string sql = SqlHelper.InitSql(DbOperationType.Update, "Blogger", columns, " WHERE Id=@Id");
            return CurrentDbOperation.ExecuteNonQuery(sql, parameters.ToArray());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>状态信息</returns>
        public KeyValuePair<string, string> ProcDelete(int id)
        {
            var outPara = SqlHelper.GetOutParameter<SqlParameter>(
                new OutParaInfo[]
                {
                    new OutParaInfo("@state", DbType.String, 50),
                    new OutParaInfo("@message", DbType.String, 50)
                }).ToList();
            var inPara = SqlHelper.GetParameter<SqlParameter>(new { Id = id, tableName = "Blogger" });
            var para = outPara.Union(inPara);
            CurrentDbOperation.ExecuteNonQuery("Delete_data", CommandType.StoredProcedure, para.ToArray());
            return new KeyValuePair<string, string>(outPara[0].Value.ToString(), outPara[1].Value.ToString());
        }
    }
}
