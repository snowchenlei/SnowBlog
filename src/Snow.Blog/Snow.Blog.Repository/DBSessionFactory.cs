using System.Data;
using System.Data.SqlClient;

namespace Snow.Blog.Repository
{
    public class DBSessionFactory
    {
        public static IDbConnection CreateDbConnection(string connectionString)
        {
            IDbConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}