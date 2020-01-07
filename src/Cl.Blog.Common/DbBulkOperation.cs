using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace DbUtility.Utility
{
    public class DbBulkOperation
    {
        private readonly string ConnectionString = string.Empty;

        /// <summary>
        /// Sqlserver方式(使用自带的SqlBulkCopy)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="table">数据表</param>
        /// <param name="batchSize">每次插入记录数</param>
        public void SqlBulkOperation(string tableName, DataTable table, int batchSize = 10000)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = batchSize;

                int columnCount = table.Columns.Count;
                DataColumn dataColumn = null;

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    dataColumn = table.Columns[columnIndex];
                    bulkCopy.ColumnMappings.Add(dataColumn.ColumnName, dataColumn.ColumnName);
                }
                bulkCopy.WriteToServer(table);
            }

        }

        /// <summary>
        /// Oracle方式
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="table">数据表</param>
        /// <param name="batchSize">每次插入记录数</param>
        public void OracleBulkOperation(string tableName, DataTable table, int batchSize = 10000)
        {
            //TODO：需要添加引用
            using (ODAC.OracleConnection conn = new ODAC.OracleConnection(ConnectionString))
            {
                using (ODAC.OracleBulkCopy bulkCopy = new ODAC.OracleBulkCopy(ConnectionString, ODAC.OracleBulkCopyOptions.Default))
                {
                    if (table != null && table.Rows.Count > 0)
                    {
                        bulkCopy.DestinationTableName = tableName;
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            string col = table.Columns[i].ColumnName;
                            bulkCopy.ColumnMappings.Add(col, col);
                        }
                        conn.Open();
                        bulkCopy.WriteToServer(table);
                    }
                }
            }
        }

        /// <summary>
        /// Mysql方式
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="table">数据表</param>
        /// <param name="batchSize">每次插入记录数</param>
        public void MysqlBulkOperation(string tableName, DataTable table, int batchSize = 10000)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlBulkLoader bulkCopy = new MySqlBulkLoader(connection);
                bulkCopy.TableName = tableName;

                bulkCopy.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                bulkCopy.Load();
            }
        }

        /// <summary>
        /// Sqlite方式(Sqlite开启事务就很快)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="table">数据表</param>
        /// <param name="batchSize">每次插入记录数</param>        
        public void SqliteBulkOperation<T>(string tableName, DataTable dataTable, int batchSize = 10000) where T : DbParameter, new()
        {
            DbOperation operation = DbOperation.GetDbOperation(ConnectionString, DbOperation.GetProviderName(DbProviderType.SQLite));
            List<KeyValuePair<string, DbParameter[]>> sqlInfo = SqlHelper.CreateInsertInfo<T>(dataTable);
            int pageCount = (int)Math.Ceiling(batchSize * 1.0 / sqlInfo.Count());
            for (int i = 0, max = pageCount - 1; i < max; i++)
            {
                operation.Transaction(sqlInfo.Skip(i * batchSize).Take(batchSize));
            }
        }
    }
}
