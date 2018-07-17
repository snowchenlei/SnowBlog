using Cl.Blog.BLL;
using Cl.Blog.Model;
using Cl.Blog.Web.Areas.Manager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Cl.Blog.Web.Areas.Manager.Controllers
{
    public class CategoryController : BaseController
    {
        private int PageIndex = 1;
        private readonly int PageSize = 2;
        CategoryBll CategoryBll = null;
        public CategoryController()
        {
            CategoryBll = new CategoryBll();
        }

        // GET: Manager/Category
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["data"] = JsonConvert.SerializeObject(List());
            return View();
        }

        /// <summary>
        /// 获取列表显示数据
        /// </summary>
        /// <returns></returns>
        public object List(string name = "", string alias = "", int pId = -1, DateTime? startTime = null, DateTime? endTime = null)
        {
            #region 显示树级菜单
            IEnumerable<Category> categories = CategoryBll.Load<Category>(name, alias, pId, startTime, endTime);
            var parentCategories = CategoryBll.LoadParent<Category>().Select(c => new { Val = c.Id, Name = c.Name });
            int totalCount = categories.Count();
            var treeData = categories.Select(c => new { id = c.Id, pId = c.PId, name = c.Name });
            var list = categories.Skip((PageIndex - 1) * PageSize).Take(PageSize).Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                Alias = c.Alias ?? "",
                Sort = c.Sort,
                pId = c.PId
            });
            #endregion
            #region 不包含树
            //IEnumerable<Category> category = CategoryBll.LoadPage<Category>(PageSize, PageIndex);
            //顶级分类
            //var parentData = category.Where(c => c.PId == 0).Select(c => new { Id = c.Id, Text = c.Name });
            //var treeData = category.Select(c => new { Id = c.Id, pId = c.PId, name = c.Name });
            //分类信息
            //var list = category.Select(c => new { Id = c.Id, Name = c.Name, Alias = c.Alias, Sort = c.Sort, pId = c.PId });
            #endregion
            int pageCount = (int)Math.Ceiling((totalCount * 1.0) / PageSize);
            return new
            {
                list = list,
                parentData = parentCategories,
                treeData = treeData,
                pageIndex = PageIndex,
                pageCount = pageCount
            };
        }

        /// <summary>
        /// 分页列表请求
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <returns>列表结果视图</returns>
        [HttpPost]
        public JsonResult List(int pageIndex, string name, string alias, int pId, DateTime? startTime, DateTime? endTime)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            return Json(List(name, alias, pId));
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Detail(int id)
        {
            Category category = CategoryBll.LoadFirst<Category>(id);
            if(category == null)
            {
                return Json(new { status = 0, message = "数据不存在" });
            }
            return Json(new { status = 1, result = category });
        }

        [HttpPost]
        public JsonResult Add(Category category)
        {
            int pid = 0;
            int.TryParse(Request.Form["PID"].ToString(), out pid);            
            category.PId = pid <= 0 ? 0 : pid;
            if (category.Id <= 0)
            {   //增加
                if (!CategoryBll.Add(category))
                {
                    return Json(new { status = 0, message = "添加分类失败" });
                }
            }
            else
            {   //修改
                if(!CategoryBll.Edit(category))
                {
                    return Json(new { status = 0, message = "修改分类失败" });
                }
            }
            return Json(new { status = 1, result = List() });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            KeyValuePair<string, string> stateInfo = CategoryBll.ProcDelete(id);
            if(stateInfo.Key != "00")
            {
                return Json(new { status = 0, message = stateInfo.Value });
            }
            else
            {
                return Json(new { status = 1, result = List() });
            }
        }
    }
}