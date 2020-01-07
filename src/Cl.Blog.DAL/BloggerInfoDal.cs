using Cl.Blog.Common;
using Cl.Blog.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.DAL
{
    public class BloggerInfoDal : BaseDal
    {
        public T FirstOrDefault<T>(int id) where T : new()
        {
            string sql = "SELECT ID,SourceType,Title,CategoryName,Sort,HtmlEncoded,Body,Description,CreateDate,IsShow,EditDate FROM BloggerInfo WHERE IsDelete = 0 AND ID = @ID";
            return CurrentDbOperation.ExecuteReader<T>(sql, SqlHelper.GetParameter<SqlParameter>(new
            {
                ID = id
            }).ToArray()).FirstOrDefault();
        }
    }
}
