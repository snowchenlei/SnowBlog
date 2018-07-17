using Cl.Blog.BLL;
using Cl.Blog.Common;
using Cl.Blog.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cl.Blog.Web.Areas.Manager.Controllers
{
    public class BloggerController : BaseController
    {
        private int PageIndex = Convert.ToInt32(ConfigurationManager.AppSettings["pageIndex"]);
        private readonly int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
        CategoryBll CategoryBll = null;
        BloggerBll BloggerBll = null;
        public BloggerController()
        {
            CategoryBll = new CategoryBll();
            BloggerBll = new BloggerBll();
        }

        // GET: Manager/Blog
        public ActionResult Index()
        {
            ViewData["data"] = JsonConvert.SerializeObject(List());
            return View();
        }

        /// <summary>
        /// 获取列表显示数据
        /// </summary>
        /// <returns></returns>
        public object List(string title = "", int categoryId = -1, int sourceType = -1, DateTime? startTime = null, DateTime? endTime = null)
        {
            var categories = CategoryBll.Load<Category>().Select(c => new { Val = c.Id, Name = c.Name });
            int pageCount, totalCount;
            //页面需要分类等相关信息，所以不需要使用视图
            IEnumerable<Blogger> blogs = BloggerBll.LoadPage<Blogger>(new PageInfo("*", "ID,SourceType,Title,CategoryId,ViewCount,Sort,Body,Description,IsShow,CreateDate,EditDate", "Blogger", "Id", "", "Sort DESC", PageSize, PageIndex), out pageCount, out totalCount, title, categoryId, sourceType, startTime, endTime);
            var sourceTypes = ReflectHelper.GetEnumDesc<SourceType>().Select(r => new { Val = r.Key, Name = r.Value });
            //关联获取需要的数据
            var list = from b in blogs
                       join c in categories on b.CategoryId equals c.Val
                       select new
                       {
                           b.Id,
                           b.Title,
                           c.Name,
                           //b.Body,
                           b.Description,
                           b.ViewCount,
                           b.Sort,
                           sourceType = ReflectHelper.GetEnumDescByFieldName<SourceType>(b.SourceType.ToString()),
                           IsShow = b.IsShow == true ? "是" : "否",
                           CreateDate = b.CreateDate.Value.ToString("yyyy-MM-dd HH:mm"),
                           EditDate = b.EditDate.HasValue ? b.EditDate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                       };
            return new { list = list, categories = categories, sourceTypes = sourceTypes, pageIndex = PageIndex, pageCount = pageCount, totalCount= totalCount };
        }

        [HttpPost]
        public JsonResult List(string title, int categoryId, string sourceType, DateTime? startTime, DateTime? endTime, int pageIndex)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            return Json(List(title, categoryId, sourceType == null ? -1 : (int)Enum.Parse(typeof(SourceType), sourceType), startTime, endTime));
        }

        [HttpPost]
        public JsonResult Detail(int id)
        {
            Blogger blogger = BloggerBll.LoadFirst<Blogger>(id);

            if (blogger == null)
            {
                return Json(new { status = 0, message = "数据不存在" });
            }
            return Json(new
            {
                status = 1,
                result = new
                {
                    blogger.Title,
                    blogger.Description,
                    blogger.HtmlEncoded,
                    blogger.Sort,
                    Category = blogger.CategoryId,
                    SourceType = blogger.SourceType.ToString(),
                    blogger.IsShow,
                }
            });
        }

        [HttpPost]
        public JsonResult Edit(Blogger blog)
        {
            blog.Body = Server.UrlDecode(blog.Body);
            blog.Description = blog.Description ?? blog.Title;
            blog.HtmlEncoded = blog.HtmlEncoded == null ? blog.Body : Server.UrlDecode(blog.HtmlEncoded);
            if(blog.Id > 0)
            {
                 if(!BloggerBll.Edit(blog))
                {
                    return Json(new { status = 0, message = "修改博客【" + blog.Title + "】失败" });
                }
            }
            else
            {
                if(!BloggerBll.Add(blog))
                {
                    return Json(new { status = 0, message = "添加博客【" + blog.Title + "】失败" });
                }
            }
            return Json(new { status = 1, result = List() });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            KeyValuePair<string, string> stateInfo = BloggerBll.ProcDelete(id);
            if (stateInfo.Key != "00")
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