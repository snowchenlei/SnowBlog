using Cl.Blog.Common;
using Cl.Blog.DAL;
using Cl.Blog.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.BLL
{
    public class BloggerBll
    {
        BloggerDal CurrentBloggerDal = null;
        public BloggerBll()
        {
            CurrentBloggerDal = new BloggerDal();
        }

        /// <summary>
        /// 加载分页数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<T> LoadPage<T>(PageInfo pageInfo, out int pageCount, out int totalCount, string title = null, int categoryId = 0, int sourceType = 0, DateTime? startTime = null, DateTime? endTime = null) where T : class, new()
        {
            DataTable result = CurrentBloggerDal.LoadPage(pageInfo, out pageCount, out totalCount, title, categoryId, sourceType, startTime, endTime);
            return DataTableHelper.DataTableToList<T>(result);
        }

        /// <summary>
        /// 加载第一条数据
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T LoadFirst<T>(int id) where T : new()
        {
            return CurrentBloggerDal.LoadFirst<T>(id);
        }


        /// <summary>
        /// 添加博客
        /// </summary>
        /// <param name="blog">博客信息</param>
        /// <returns>是否成功</returns>
        public bool Add(Blogger blog)
        {
            return CurrentBloggerDal.Add(blog) > 0;
        }

        /// <summary>
        /// 修改博客
        /// </summary>
        /// <param name="blog">博客信息</param>
        /// <returns>是否成功</returns>
        public bool Edit(Blogger blog)
        {
            return CurrentBloggerDal.Edit(blog) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>状态信息</returns>
        public KeyValuePair<string, string> ProcDelete(int id)
        {
            return CurrentBloggerDal.ProcDelete(id);
        }

    }
}
