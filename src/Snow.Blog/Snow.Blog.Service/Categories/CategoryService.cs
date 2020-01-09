using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snow.Blog.IDAL.Categories;

namespace Snow.Blog.Service.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryDal;

        public CategoryService(ICategoryRepository categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task<Dictionary<int, string>> GetNavSelectList()
        {
            return await _categoryDal.GetCategoriesAsync();
        }
    }
}