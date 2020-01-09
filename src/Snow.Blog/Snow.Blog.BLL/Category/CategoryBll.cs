using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Blog.BLL.Category
{
    public class CategoryBll : ICategoryBll
    {
        public Task<Dictionary<int, string>> GetSelectList()
        {
            return Task.FromResult(new Dictionary<int, string>()
            {
                { 1, "Html" },
                { 2, "UWP" },
                { 3, "C#" }
            });
        }
    }
}