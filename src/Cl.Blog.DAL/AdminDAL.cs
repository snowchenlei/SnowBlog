using Cl.Blog.Common;
using Cl.Blog.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.DAL
{
    public class AdminDal:BaseDal
    {
        /// <summary>
        /// 管理员登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns>用户信息</returns>
        public DataTable Login(string userName, string passWord)
        {
            string sql = "SELECT * FROM Admin WHERE userName=@userName AND passWord=@passWord";
            return CurrentDbOperation.Query(sql, SqlHelper.GetParameter<SqlParameter>(new { userName = userName, passWord = passWord }).ToArray());
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId">需要重置的用户Id</param>
        /// <param name="newPwd">新密码</param>
        /// <returns>是否成功</returns>
        public int ResetPassword(int Id, string newPwd)
        {
            string sql = "UPDATE TABLE Admin SET passWord=@passWord WHERE Id=@Id";
            return CurrentDbOperation.ExecuteNonQuery(sql, SqlHelper.GetParameter<SqlParameter>(new { Id = Id, passWord = newPwd }).ToArray());
        }
    }
}
