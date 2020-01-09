using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Blog.BLL.Category
{
    public interface ICategoryBll
    {
        Task<Dictionary<int, string>> GetSelectList();
    }
}