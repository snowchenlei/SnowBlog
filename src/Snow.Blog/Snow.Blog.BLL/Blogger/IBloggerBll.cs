using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Blog.BLL.Blogger
{
    public interface IBloggerBll
    {
        Task GetPagedAsync();
    }
}