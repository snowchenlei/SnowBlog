using Cl.Blog.Common;
using Cl.Blog.DAL;
using Cl.Blog.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.BLL
{
    /// <summary>
    /// 管理员表操作
    /// </summary>
    public class AdminBll
    {
        AdminDal CurrentAdminDAL;
        public AdminBll()
        {
            CurrentAdminDAL = new AdminDal();
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns>登陆用户信息</returns>
        public Admin Login(string userName,string passWord)
        {
            DataTable result = CurrentAdminDAL.Login(userName, passWord);
            return DataTableHelper.DataTableToModel<Admin>(result);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="newPwd">新密码</param>
        /// <returns>是否成功</returns>
        public bool ResetPassword(int Id, string newPwd)
        {
            return CurrentAdminDAL.ResetPassword(Id, newPwd) > 0;
        }
    }
}
