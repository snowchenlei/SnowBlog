using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Snow.Blog.IDAL.Categories;
using Snow.Blog.Model.DataBase;
using Snow.Blog.Repository;

namespace Snow.Blog.SqlServer
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Dictionary<int, string>> GetCategoriesAsync(bool isNav = true)
        {
            string whereSql = " AND IsDelete=0";
            if (isNav)
            {
                whereSql += " AND IsNavShow=1";
            }
            return (await GetAsync(whereSql, columns: "ID, Name"))
                .ToDictionary(key => key.Id, value => value.Name);
        }
    }
}