using Cl.Blog.DAL;
using Cl.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.BLL
{
    public class BloggerInfoBll
    {
        private BloggerInfoDal CurrentBloggerInfoDal = null;
        public BloggerInfoBll()
        {
            CurrentBloggerInfoDal = new BloggerInfoDal();
        }

        public T FirstOrDefault<T>(int id) where T : new()
        {
            return CurrentBloggerInfoDal.FirstOrDefault<T>(id);
        }
    }
}
