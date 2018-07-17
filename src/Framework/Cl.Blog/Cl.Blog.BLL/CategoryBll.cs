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
    public class CategoryBll
    {
        CategoryDal CurrentCategoryDal = null;
        public CategoryBll()
        {
            CurrentCategoryDal = new CategoryDal();
        }
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="pId">父级编号</param>
        /// <param name="sort">排序(对同一父级有效)</param>
        /// <returns>受影响的行数</returns>
        public bool Add(Category category)
        {
            return CurrentCategoryDal.Add(category) > 0;
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="name">名称</param>
        /// <param name="pId">父级编号</param>
        /// <param name="sort">排序(对同一父级有效)</param>
        /// <returns>受影响的行数</returns>
        public bool Edit(Category category)
        {
            return CurrentCategoryDal.Edit(category) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>状态信息</returns>
        public KeyValuePair<string, string> ProcDelete(int id)
        {
            return CurrentCategoryDal.ProcDelete(id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns></returns>
        public IEnumerable<T> Load<T>(int pageSize, int pageIndex, out int pageCount, out int totalCount) where T : class, new()
        {
            DataTable result = CurrentCategoryDal.Load(pageSize, pageIndex, out pageCount, out totalCount);
            return DataTableHelper.DataTableToList<T>(result);
        }

        /// <summary>
        /// 获取首条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T LoadFirst<T>(int id) where T : class, new()
        {
            DataTable result = CurrentCategoryDal.Load(id);
            return DataTableHelper.DataTableToList<T>(result).FirstOrDefault();
        }


        /// <summary>
        /// 加载父分类
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> LoadParent<T>() where T : class, new()
        {
            DataTable result = CurrentCategoryDal.LoadParent();
            return DataTableHelper.DataTableToList<T>(result);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>获取到的分类</returns>
        public IEnumerable<T> Load<T>(string name, string alias, int pId, DateTime? startTime, DateTime? endTime) where T : class, new()
        {
            DataTable result = CurrentCategoryDal.Load(name , alias, pId, startTime, endTime);
            return DataTableHelper.DataTableToList<T>(result);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>获取到的分类</returns>
        public IEnumerable<T> Load<T>() where T : class, new()
        {
            DataTable result = CurrentCategoryDal.Load();
            return DataTableHelper.DataTableToList<T>(result);
        }

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists<T>(string categoryName)
        {
            return CurrentCategoryDal.Exists<T>(categoryName);
        }

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="whereObj">过滤sql条件</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists<T>(int id)
        {
            return CurrentCategoryDal.Exists<T>(id);
        }
    }
}
