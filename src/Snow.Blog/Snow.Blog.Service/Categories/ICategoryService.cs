using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Blog.Service.Categories
{
    public interface ICategoryService
    {
        Task<Dictionary<int, string>> GetNavSelectList();
    }
}