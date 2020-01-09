using System.Data;
using System.Data.SqlClient;

namespace Snow.Blog.DAL
{
    public class DBSessionFactory
    {
        public static IDbConnection CreateDbConnection(string connectionString)
        {
            IDbConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}