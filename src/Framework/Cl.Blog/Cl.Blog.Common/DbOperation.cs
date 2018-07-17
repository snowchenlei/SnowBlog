using Cl.Blog.Model.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace Cl.Blog.Common
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DbOperation:IDisposable
    {
        #region 数据初始化
        /// <summary>
        /// 数据库操作工厂类
        /// </summary>
        private readonly DbProviderFactory Provider = null;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private readonly string ConnectionString = string.Empty;
        #region 获取唯一单例对象
        /// <summary>
        /// 唯一数据库操作类对象
        /// </summary>
        private static DbOperation _dbOperation;
        /// <summary>
        /// 创建DbOperation对象
        /// </summary>
        /// <param name="configName">配置键名称</param>
        /// <returns>DbOperation对象</returns>
        public static DbOperation GetDbOperation(string configName)
        {            
            if (_dbOperation == null)
            {
                _dbOperation = new DbOperation(configName);
            }
            return _dbOperation;
        }
        /// <summary>
        /// 创建DbOperation对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="providerName">数据库类型</param>
        /// <returns>DbOperation对象</returns>
        public static DbOperation GetDbOperation(string connectionString, string providerName)
        {
            if (_dbOperation == null)
            {
                _dbOperation = new DbOperation(connectionString, providerName);
            }
            return _dbOperation;
        } 
        #endregion

        #region 构造函数
        private DbOperation(string configName)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[configName];
            Provider = DbProviderFactories.GetFactory(settings.ProviderName);
            ConnectionString = settings.ConnectionString;
        }
        private DbOperation(string connectionString, string providerName)
        {
            Provider = DbProviderFactories.GetFactory(providerName);
            ConnectionString = connectionString;
        } 
        #endregion
        #endregion

        #region 辅助方法
        /// <summary>
        /// 根据数据库获取ProviderName
        /// </summary>
        /// <param name="providerType">数据库类型</param>
        /// <returns>ProviderName</returns>
        public static string GetProviderName(DbProviderType providerType)
        {
            string providerName = string.Empty;
            switch (providerType)
            {
                case DbProviderType.SqlServer:
                    providerName = "System.Data.SqlClient";
                    break;
                case DbProviderType.MySql:
                    providerName = "MySql.Data.MySqlClient";
                    break;
                case DbProviderType.SQLite:
                    providerName = "System.Data.SQLite";
                    break;
                case DbProviderType.Oracle:
                    providerName = "System.Data.OracleClient";
                    break;
                case DbProviderType.Access:
                case DbProviderType.ODBC:
                    providerName = "System.Data.Odbc";
                    break;
                case DbProviderType.OleDb:
                    providerName = "System.Data.OleDb";
                    break;
                case DbProviderType.Firebird:
                    providerName = "FirebirdSql.Data.Firebird";
                    break;
                case DbProviderType.PostgreSql:
                    providerName = "Npgsql";
                    break;
                case DbProviderType.DB2:
                    providerName = "IBM.Data.DB2.iSeries";
                    break;
                case DbProviderType.Informix:
                    providerName = "IBM.Data.Informix";
                    break;
                case DbProviderType.SqlServerCe:
                    providerName = "System.Data.SqlServerCe";
                    break;
                default:
                    providerName = "System.Data.Odbc";
                    break;
            }
            return providerName;
        }

        /// <summary>
        /// 创建DbCommand对象
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">sql语句的类型</param>
        /// <param name="parameters">执行sql语句所需要的参数</param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(string sql, CommandType commandType, DbConnection connection, params DbParameter[] parameters)
        {
            connection.ConnectionString = ConnectionString;
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = commandType;
            command.Parameters.AddRange(parameters);
            return command;
        }

        /// <summary>
        /// 创建DbCommand对象
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">sql语句的类型</param>
        /// <param name="parameters">执行sql语句所需要的参数</param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(CommandType commandType, DbConnection connection)
        {
            connection.ConnectionString = ConnectionString;
            DbCommand command = connection.CreateCommand();
            command.CommandType = commandType;
            return command;
        }

        #region DataReader转实体
        /// <summary>
        /// DataReader转泛型
        /// </summary>
        /// <typeparam name="T">传入的实体类</typeparam>
        /// <param name="objReader">DataReader对象</param>
        /// <returns></returns>
        private static IList<T> ReaderToList<T>(IDataReader objReader)
        {
            using (objReader)
            {
                IList<T> list = new List<T>();

                //获取传入的数据类型
                Type modelType = typeof(T);

                //遍历DataReader对象
                while (objReader.Read())
                {
                    //使用与指定参数匹配最高的构造函数，来创建指定类型的实例
                    T model = Activator.CreateInstance<T>();
                    for (int i = 0; i < objReader.FieldCount; i++)
                    {
                        //判断字段值是否为空或不存在的值
                        if (!IsNullOrDBNull(objReader[i]))
                        {
                            //匹配字段名
                            PropertyInfo propertyInfo = modelType.GetProperty(objReader.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (propertyInfo != null)
                            {
                                if (propertyInfo.PropertyType.FullName == "System.Boolean" && !(objReader[i] is bool))
                                {
                                    propertyInfo.SetValue(model, objReader[i].ToString() == "1" ? true : false);
                                }
                                else
                                {
                                    //绑定实体对象中同名的字段  
                                    //propertyInfo.SetValue(model, CheckType(objReader[i], propertyInfo.PropertyType), null);
                                    propertyInfo.SetValue(model, objReader[i]);
                                }
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
        }

        /// <summary>
        /// 对可空类型进行判断转换(*要不然会报错)
        /// </summary>
        /// <param name="value">DataReader字段的值</param>
        /// <param name="conversionType">该字段的类型</param>
        /// <returns></returns>
        private static object CheckType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }            
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// 判断指定对象是否是有效值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsNullOrDBNull(object obj)
        {
            return (obj == null || (obj is DBNull)) ? true : false;
        }

        /// <summary>
        /// DataReader转模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objReader"></param>
        /// <returns></returns>
        private static T ReaderToModel<T>(IDataReader objReader)
        {

            using (objReader)
            {
                if (objReader.Read())
                {
                    Type modelType = typeof(T);
                    int count = objReader.FieldCount;
                    T model = Activator.CreateInstance<T>();
                    for (int i = 0; i < count; i++)
                    {
                        if (!IsNullOrDBNull(objReader[i]))
                        {
                            PropertyInfo pi = modelType.GetProperty(objReader.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (pi != null)
                            {
                                pi.SetValue(model, CheckType(objReader[i], pi.PropertyType), null);
                            }
                        }
                    }
                    return model;
                }
            }
            return default(T);
        }
        #endregion
        #endregion

        #region 主要方法
        #region Query
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>包含数据的DataTable</returns>
        public DataTable Query(string sql, params DbParameter[] parameters)
        {
            return Query(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>包含数据的DataTable</returns>
        public DataTable Query(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            DataTable resultTable = new DataTable();
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    connection.Open();
                    DbDataAdapter adapter = Provider.CreateDataAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(resultTable);
                }
            }
            return resultTable;
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>包含数据的DataTable</returns>
        public async Task<DataTable> QueryAsync(string sql, params DbParameter[] parameters)
        {
            return await QueryAsync(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>包含数据的DataTable</returns>
        public async Task<DataTable> QueryAsync(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            DataTable resultTable = new DataTable();
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, CommandType.Text, connection, parameters))
                {
                    await connection.OpenAsync();
                    DbDataAdapter adapter = Provider.CreateDataAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(resultTable);
                }
            }
            return resultTable;
        }
        #endregion

        #region GetSchema
        /// <summary>
        /// 获取数据源架构信息
        /// </summary>
        /// <param name="schemaType">要获取的架构信息类型</param>
        /// <returns></returns>
        public DataTable GetSchema()
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                return connection.GetSchema();
            }
        }

        /// <summary>
        /// 获取数据源架构信息
        /// </summary>
        /// <param name="architectureType">要获取的架构信息类型</param>
        /// <returns></returns>
        public DataTable GetSchema(ArchitectureType architectureType)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                return connection.GetSchema(architectureType.ToString());
            }
        }

        /// <summary>
        /// 获取数据源架构信息
        /// </summary>
        /// <param name="architectureType">要获取的架构信息类型</param>
        /// <returns></returns>
        public DataTable GetSchema(ArchitectureType architectureType, params string[] restrictionValues)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                return connection.GetSchema(architectureType.ToString(), restrictionValues);
            }
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// Reader查询获取数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string sql, params DbParameter[] parameters)
        {
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// Reader查询获取数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand commnd = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    commnd.Connection.Open();
                    using (DbDataReader reader = commnd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        DataTable result = new DataTable();
                        result.Load(reader);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Reader查询获取数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteReader<T>(string sql, params DbParameter[] parameters) where T : new()
        {
            return ExecuteReader<T>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// Reader查询获取数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteReader<T>(string sql, CommandType commandType, params DbParameter[] parameters) where T:new()
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand commnd = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    commnd.Connection.Open();
                    using (DbDataReader reader = commnd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        return ReaderToList<T>(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Reader查询获取数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteReaderAsync(string sql, params DbParameter[] parameters)
        {
            return await ExecuteReaderAsync(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// Reader查询获取数据
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteReaderAsync(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand commnd = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    await commnd.Connection.OpenAsync();
                    using (DbDataReader reader = await commnd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        DataTable result = new DataTable();
                        result.Load(reader);
                        return result;
                    }
                }
            }
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 获取首行首列的数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params DbParameter[] parameters)
        {
            return ExecuteScalar<T>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取首行首列的数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    command.Connection.Open();
                    object o = command.ExecuteScalar();
                    if(o is T)
                    {
                        return (T)o;
                    }
                    else
                    {
                        throw new InvalidCastException("类型转换失败");
                    }
                }
            }
        }

        /// <summary>
        /// 异步获取首行首列的数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, params DbParameter[] parameters) where T : class
        {
            return await ExecuteScalarAsync<T>(sql, parameters);
        }

        /// <summary>
        /// 异步获取首行首列的数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">要执行sql语句的类型</param>
        /// <param name="parameters">要执行sql语句的参数</param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, CommandType commandType, params DbParameter[] parameters) where T : class
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    await command.Connection.OpenAsync();
                    object o = await command.ExecuteScalarAsync();
                    if (o is T)
                    {
                        return o as T;
                    }
                    else
                    {
                        throw new InvalidCastException("类型转换失败");
                    }
                }
            }
        }
        #endregion

        #region Transaction
        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        public void Transaction(string sql, params DbParameter[] parameters)
        {
            Transaction(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        public void Transaction(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    DbTransaction transaction = null;
                    try
                    {
                        command.Connection.Open();
                        transaction = command.Connection.BeginTransaction();
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        public void TransactionAsync(string sql, params DbParameter[] parameters)
        {
            TransactionAsync(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        public async void TransactionAsync(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    DbTransaction transaction = null;
                    try
                    {
                        await command.Connection.OpenAsync();
                        transaction = command.Connection.BeginTransaction();
                        command.Transaction = transaction;
                        await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        public void Transaction(Dictionary<string, DbParameter[]> sqlInfo)
        {
            Transaction(sqlInfo, CommandType.Text);
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        public void Transaction(Dictionary<string,DbParameter[]> sqlInfo, CommandType commandType)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(commandType, connection))
                {
                    DbTransaction transaction = null;
                    try
                    {
                        command.Connection.Open();
                        transaction = command.Connection.BeginTransaction();
                        command.Transaction = transaction;
                        foreach (KeyValuePair<string, DbParameter[]> item in sqlInfo)
                        {
                            command.CommandText = item.Key;
                            command.Parameters.AddRange(item.Value);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        public void TransactionAsync(Dictionary<string, DbParameter[]> sqlInfo)
        {
            TransactionAsync(sqlInfo, CommandType.Text);
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        public async void TransactionAsync(Dictionary<string, DbParameter[]> sqlInfo, CommandType commandType)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(commandType, connection))
                {
                    DbTransaction transaction = null;
                    try
                    {
                        await command.Connection.OpenAsync();
                        transaction = command.Connection.BeginTransaction();
                        command.Transaction = transaction;
                        foreach (KeyValuePair<string, DbParameter[]> item in sqlInfo)
                        {
                            command.CommandText = item.Key;
                            command.Parameters.AddRange(item.Value);
                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        public void Transaction(IEnumerable<KeyValuePair<string, DbParameter[]>> sqlInfo)
        {
            Transaction(sqlInfo, CommandType.Text);
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        public void Transaction(IEnumerable<KeyValuePair<string, DbParameter[]>> sqlInfo, CommandType commandType)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(commandType, connection))
                {
                    DbTransaction transaction = null;
                    try
                    {
                        command.Connection.Open();
                        transaction = command.Connection.BeginTransaction();
                        command.Transaction = transaction;
                        foreach (KeyValuePair<string, DbParameter[]> item in sqlInfo)
                        {
                            command.CommandText = item.Key;
                            command.Parameters.AddRange(item.Value);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        public void TransactionAsync(IEnumerable<KeyValuePair<string, DbParameter[]>> sqlInfo)
        {
            TransactionAsync(sqlInfo, CommandType.Text);
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqlInfo">要执行的sql语句的相关信息</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        public async void TransactionAsync(IEnumerable<KeyValuePair<string, DbParameter[]>> sqlInfo, CommandType commandType)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(commandType, connection))
                {
                    DbTransaction transaction = null;
                    try
                    {
                        await command.Connection.OpenAsync();
                        transaction = command.Connection.BeginTransaction();
                        command.Transaction = transaction;
                        foreach (KeyValuePair<string, DbParameter[]> item in sqlInfo)
                        {
                            command.CommandText = item.Key;
                            command.Parameters.AddRange(item.Value);
                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region 大数据插入        
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql, params DbParameter[] parameters)
        {
            return ExecuteNonQuery(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    command.Connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 异步执行增删改
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params DbParameter[] parameters)
        {
            return await ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 异步执行增删改
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <param name="parameters">执行SQL语句所需要的参数</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                using (DbCommand command = CreateDbCommand(sql, commandType, connection, parameters))
                {
                    await command.Connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            //connection.Close();
        }
    }
    
}
