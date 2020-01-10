using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Snow.Blog.IRepository.Bloggers;
using Snow.Blog.Model.DataBase;
using Snow.Blog.Repository;

namespace Snow.Blog.SqlServer
{
    public class BloggerRepository : BaseRepository<Blogger>, IBloggerRepository
    {
        public BloggerRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> GetCountAsync()
        {
            string sql = $"SELECT COUNT(1) FROM {TableName} WHERE IsDelete=0 AND IsShow=1";
            return await ExecuteScalarAsync<int>(sql);
        }

        public async Task<Blogger> GetDetailAsync(int id)
        {
            string sql = @$"SELECT TOP 1 b.*, c.Name FROM {TableName} AS b
                        LEFT JOIN Category AS c ON b.CategoryId=c.Id
                        WHERE b.IsDelete=0 AND b.IsShow=1 AND b.Id=@Id";
            using (IDbConnection connection = DBSessionFactory.CreateDbConnection(ConnectionString))
            {
                var result = await connection.QueryAsync<Blogger, Category, Blogger>(sql, (b, c) => { b.Category = c; return b; }, new
                {
                    Id = id
                }, splitOn: "Name");
                return result.FirstOrDefault();
            }
        }

        protected override void InitialPageSql(out string countQuery, out string selectQuery)
        {
            countQuery = $@"SELECT COUNT(1)
                      FROM [{TableName}]
                      /**where**/";

            selectQuery = $@"SELECT *
              FROM  (SELECT ROW_NUMBER() OVER ( /**orderby**/ ) AS RowNum, b.*, c.Name
                   FROM   [{TableName}] AS b
                    LEFT JOIN Category AS c ON b.CategoryId=c.ID
                   /**where**/
                  ) AS RowConstrainedResult
              WHERE  RowNum >= ((@PageIndex-1) * @PageSize + 1 )
                AND RowNum <= (@PageIndex) * @PageSize
              ORDER BY RowNum";
        }

        protected override Task<IEnumerable<Blogger>> QueryAsync(string sql, object parameters = null, IDbConnection connection = null)
        {
            if (connection == null)
            {
                using (connection = DBSessionFactory.CreateDbConnection(ConnectionString))
                {
                    return connection.QueryAsync<Blogger, Category, Blogger>(sql, (b, c) => { b.Category = c; return b; }, parameters, splitOn: "Name");
                }
            }
            else
            {
                return connection.QueryAsync<Blogger, Category, Blogger>(sql, (b, c) => { b.Category = c; return b; }, parameters, splitOn: "Name");
            }
        }
    }
}