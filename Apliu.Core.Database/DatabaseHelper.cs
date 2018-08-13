using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Apliu.Core.Database
{
    public class DatabaseHelper
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType databaseType = DatabaseType.SqlServer;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxPool = 10;
        /// <summary>
        /// 最小连接数
        /// </summary>
        public int MinPool = 3;
        /// <summary>
        /// 连接等待时间 单位秒
        /// </summary>
        public int Conn_Timeout = 10;
        /// <summary>
        /// 连接的生命周期 单位秒
        /// </summary>
        public int Conn_Lifetime = 120;

        private string _databaseConnection = String.Empty;
        /// <summary>
        /// 数据库链接字符串 System.Data.SqlClient.SqlConnectionStringBuilder
        /// </summary>
        public string DatabaseConnection
        {
            get
            {
                return CreateDatabaseConnectionStr(_databaseConnection);
            }
            set
            {
                if (value.EndsWith(";")) _databaseConnection = value.Substring(0, value.Length);
                else _databaseConnection = value;
            }
        }

        /// <summary>
        /// 初始化数据库操作对象
        /// </summary>
        /// <param name="ConnectionString"></param>
        public DatabaseHelper(DatabaseType DatabaseType, String ConnectionString)
        {
            this._databaseConnection = ConnectionString;
            this.databaseType = DatabaseType;
        }

        /// <summary>
        /// 执行SQL语句查询数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>结果集</returns>
        public DataSet GetData(string Sql)
        {
            return GetDataExecute(CommandType.Text, Sql, 30, null);
        }

        /// <summary>
        /// 执行SQL语句更新数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>受影响的行数</returns>
        public int PostData(string Sql)
        {
            return PostDataExecute(CommandType.Text, Sql, 30, null);
        }

        #region 数据库事务相关处理
        /// <summary>
        /// 数据库事务范围
        /// </summary>
        private TransactionScope transactionScope = null;

        /// <summary>
        /// 开启数据库事务
        /// </summary>
        /// <param name="seconds">事务超时时间 单位秒</param>
        public void BeginTransaction(int seconds)
        {
            TransactionOptions transactionOption = new TransactionOptions
            {
                //设置事务隔离级别
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                // 设置事务超时时间
                Timeout = new TimeSpan(0, 0, seconds)
            };
            transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOption);

            //当时间超过之后，主动注销该事务
            Task.Factory.StartNew(() => { Thread.Sleep(seconds * 1000); Dispose(); });
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Complete()
        {
            if (transactionScope != null)
            {
                transactionScope.Complete();
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        /// <summary>
        /// 撤销事务
        /// </summary>
        public void Dispose()
        {
            if (transactionScope != null)
            {
                transactionScope.Dispose();
                transactionScope = null;
            }
        }
        #endregion

        #region 执行Transact-SQL语句或存储过程，并返回查询结果或受影响的行数
        /// <summary>
        /// 执行 Transact-SQL 语句，并返回受影响的行数
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="commandText">Sql语句或存储过程</param>
        /// <param name="commandTimeout">语句执行的超时时间</param>
        /// <param name="commandParameters">语句参数 </param>
        /// <returns>返回受影响的行数</returns>
        public int PostDataExecute(CommandType commandType, string commandText, int commandTimeout, params object[] commandParameters)
        {
            int affected = -1;
            try
            {
                //SQL语句以分号结束
                if (!commandText.Trim().EndsWith(";")) commandText = commandText.Trim() + ";";

                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        break;
                    case DatabaseType.Oracle://Oracle SQL语句必须加上Begin End
                        if (!commandText.Trim().ToUpper().EndsWith("END;")) commandText = "begin " + commandText + " end;";
                        break;
                    case DatabaseType.MySql:
                        break;
                    default:
                        break;
                }

                using (DbConnection dbConnection = CreateDbConnection(DatabaseConnection))
                {
                    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                    using (DbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = commandText;
                        dbCommand.CommandTimeout = commandTimeout;
                        dbCommand.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            foreach (DbParameter parm in commandParameters)
                                dbCommand.Parameters.Add(parm);
                        }
                        affected = dbCommand.ExecuteNonQuery();
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                affected = -1;
                throw ex;
            }

            #region 过期方式
            //switch (databaseType)
            //{
            //    case DatabaseType.SqlServer:
            //        SqlServerExecuteNonQuery(commandType, commandText, commandTimeout, out val, commandParameters);
            //        break;
            //    case DatabaseType.Oracle:
            //        break;
            //    case DatabaseType.MySql:
            //        MySqlExecuteNonQuery(commandType, commandText, commandTimeout, out val, commandParameters);
            //        break;
            //    case DatabaseType.Access:
            //        break;
            //    default:
            //        break;
            //}
            #endregion

            return affected;
        }

        /// <summary>
        /// 执行Transact-SQL语句或存储过程，并返回查询结果
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="commandText">Sql语句或存储过程</param>
        /// <param name="commandTimeout">语句执行的超时时间</param>
        /// <param name="commandParameters">语句参数</param>
        /// <returns>返回结果集</returns>
        public DataSet GetDataExecute(CommandType commandType, string commandText, int commandTimeout, params object[] commandParameters)
        {
            DataSet dsData = null;
            try
            {
                using (DbConnection dbConnection = CreateDbConnection(DatabaseConnection))
                {
                    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                    using (DbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = commandText;
                        dbCommand.CommandTimeout = commandTimeout;
                        dbCommand.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            //sqlCommand.Parameters.Clear();
                            foreach (DbParameter parm in commandParameters)
                                dbCommand.Parameters.Add(parm);
                        }
                        DbDataAdapter da = CreateDbDataAdapter(dbCommand);
                        dsData = new DataSet();
                        da.Fill(dsData);
                    }
                }
            }
            catch (Exception ex)
            {
                dsData = null;
                throw ex;
            }

            #region 过期方式
            //switch (databaseType)
            //{
            //    case DatabaseType.SqlServer:
            //        SqlServerDataAdapter(commandType, commandText, commandTimeout, out dsData, commandParameters);
            //        break;
            //    case DatabaseType.Oracle:
            //        break;
            //    case DatabaseType.MySql:
            //        MySqlDataAdapter(commandType, commandText, commandTimeout, out dsData, commandParameters);
            //        break;
            //    case DatabaseType.Access:
            //        break;
            //    default:
            //        break;
            //}
            #endregion

            return dsData;
        }

        /// <summary>
        /// 执行BulkCopy指令将DataTable插入到数据库，并返回受影响的行数
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async ValueTask<Int32> InsertTableAsync(DataTable dataTable, Int32 timeout)
        {
            /*注意必须要有：Persist Security Info=True
            SqlBulkCopy
            OracleBulkCopy
            MySqlBulkLoader*/
            if (dataTable == null || dataTable.Rows.Count <= 0) return 0;
            if (string.IsNullOrEmpty(dataTable.TableName)) throw new ArgumentException("TableName不能为空");

            Int32 affected = -1;
            try
            {
                using (DbConnection dbConnection = CreateDbConnection(DatabaseConnection))
                {
                    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                    switch (databaseType)
                    {
                        case DatabaseType.SqlServer:
                            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(dbConnection as SqlConnection)
                            {
                                //BatchSize = 2000,
                                BulkCopyTimeout = timeout,
                                DestinationTableName = dataTable.TableName
                            };
                            //sqlBulkCopy.SqlRowsCopied += SqlBulkCopy_SqlRowsCopied;
                            await sqlBulkCopy.WriteToServerAsync(dataTable);
                            affected = dataTable.Rows.Count;
                            sqlBulkCopy.Close();
                            break;
                        case DatabaseType.MySql:
                            String tempFilePath = Path.GetTempFileName();
                            File.WriteAllText(tempFilePath, DataTableToCsv(dataTable));

                            MySqlBulkLoader mySqlBulkLoader = new MySqlBulkLoader(dbConnection as MySqlConnection)
                            {
                                FieldTerminator = ",",
                                FieldQuotationCharacter = '"',
                                EscapeCharacter = '"',
                                LineTerminator = "\r\n",
                                FileName = tempFilePath,
                                NumberOfLinesToSkip = 0,
                                Timeout = timeout,
                                TableName = dataTable.TableName,
                            };
                            affected = await mySqlBulkLoader.LoadAsync();
                            if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
                            break;
                        case DatabaseType.Oracle:
                        //OracleBulkCopy bulkCopy = new OracleBulkCopy(connOrcleString, OracleBulkCopyOptions.UseInternalTransaction);   //用其它源的数据有效批量加载Oracle表中
                        //bulkCopy.BatchSize = 100000;
                        //bulkCopy.BulkCopyTimeout = 260;
                        //bulkCopy.DestinationTableName = targetTable;    //服务器上目标表的名称
                        //bulkCopy.BatchSize = dt.Rows.Count;   //每一批次中的行数
                        //bulkCopy.WriteToServer(dt);   //将提供的数据源中的所有行复制到目标表中
                        default:
                            throw new Exception("暂不支持该数据库类型");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return affected;
        }
        #endregion

        /// <summary>
        /// 初始化数据库链接字符串
        /// </summary>
        /// <param name="beginConnectionStr"></param>
        /// <returns></returns>
        private String CreateDatabaseConnectionStr(String beginConnectionStr)
        {
            String databaseConnectionStr = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    databaseConnectionStr = beginConnectionStr + ";"
                                        + "Max Pool Size=" + MaxPool + ";"
                                        + "Min Pool Size=" + MinPool + ";"
                                        + "Connect Timeout=" + Conn_Timeout + ";"
                                        + "Connection Lifetime=" + Conn_Lifetime + ";"
                                        + "Pooling =True;";
                    break;
                case DatabaseType.MySql:
                    databaseConnectionStr = beginConnectionStr + ";"
                                        + "maxpoolsize=" + MaxPool + ";"
                                        + "minpoolsize=" + MinPool + ";"
                                        + "connectiontimeout=" + Conn_Timeout + ";"
                                        + "connectionlifetime=" + Conn_Lifetime + ";"
                                        + "pooling=True;SslMode = none;";
                    break;
                case DatabaseType.Oracle:
                    databaseConnectionStr = beginConnectionStr + ";"
                                        + "MIN POOL SIZE=" + MinPool + ";"
                                        + "MAX POOL SIZE=" + MaxPool + ";"
                                        + "CONNECTION TIMEOUT=" + Conn_Timeout + ";"
                                        + "CONNECTION LIFETIME=" + Conn_Lifetime + ";"
                                        + "POOLING=True;";
                    break;
                default:
                    throw new Exception("数据库类型有误或未初始化 databaseType：" + databaseType.ToString());
            }
            return databaseConnectionStr;
        }

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="databaseConnection"></param>
        /// <returns></returns>
        private DbConnection CreateDbConnection(String databaseConnection)
        {
            DbConnection dbConnection = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbConnection = new SqlConnection(databaseConnection);
                    break;
                case DatabaseType.Oracle:
                    dbConnection = new OracleConnection(databaseConnection);
                    break;
                case DatabaseType.MySql:
                    dbConnection = new MySqlConnection(databaseConnection);
                    break;
                case DatabaseType.OleDb:
#if NET20 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET49
                    dbConnection = OleDbFactory.CreateOleDbConnection(databaseConnection);
#else
                    throw new Exception("必须是.NET Framework框架才可以使用，Database Type：" + databaseType.ToString());
#endif
                default:
                    throw new Exception("数据库类型有误或未初始化，Database Type：" + databaseType.ToString());
            }
            return dbConnection;
        }

        /// <summary>
        /// 获取用于填充 System.Data.DataSet 的对象
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        private DbDataAdapter CreateDbDataAdapter(DbCommand dbCommand)
        {
            DbDataAdapter dbDataAdapter;

            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbDataAdapter = new SqlDataAdapter(dbCommand as SqlCommand);
                    break;
                case DatabaseType.Oracle:
                    dbDataAdapter = new OracleDataAdapter(dbCommand as OracleCommand);
                    break;
                case DatabaseType.MySql:
                    dbDataAdapter = new MySqlDataAdapter(dbCommand as MySqlCommand);
                    break;
                case DatabaseType.OleDb:
#if NET20 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET49
                    dbDataAdapter = OleDbFactory.CreateOleDbDataAdapter(dbCommand);
#else
                    throw new Exception("必须是.NET Framework框架才可以使用，Database Type：" + databaseType.ToString());
#endif
                default:
                    throw new Exception("数据库类型有误或未初始化 databaseType：" + databaseType.ToString());
            }

            return dbDataAdapter;
        }

        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Sql初始化参数 MakeParam("@name" , 枚举.VarChar.ToString() , 50 ,value) as SqlParameter
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">SqlDbType/MySqlDbType枚举</param>
        /// <param name="size">参数长度</param>
        /// <param name="direction">参数类型</param>
        /// <param name="value">参数值</param>
        /// <returns>SqlParameter/MySqlParameter类型</returns>
        public DbParameter MakeParam(string paramName, String dbType, Int32 size, ParameterDirection direction, object value)
        {
            DbParameter dbParameter = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbParameter = new SqlParameter();
                    ((SqlParameter)dbParameter).SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), dbType);
                    break;
                case DatabaseType.Oracle:
                    dbParameter = new OracleParameter();
                    ((OracleParameter)dbParameter).OracleDbType = (OracleDbType)Enum.Parse(typeof(OracleDbType), dbType);
                    break;
                case DatabaseType.MySql:
                    dbParameter = new MySqlParameter();
                    ((MySqlParameter)dbParameter).MySqlDbType = (MySqlDbType)Enum.Parse(typeof(MySqlDbType), dbType);
                    break;
                case DatabaseType.OleDb:
#if NET20 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET49
                    dbParameter = OleDbFactory.CreateOleDbParameter(paramName, (DbType)Enum.Parse(typeof(DbType), dbType), size, value);
#else
                    throw new Exception("必须是.NET Framework框架才可以使用，Database Type：" + databaseType.ToString());
#endif
                default:
                    break;
            }
            dbParameter.ParameterName = paramName;
            if (size > 0) dbParameter.Size = size;
            dbParameter.Direction = direction;

            if (!(direction == ParameterDirection.Output && value == null)) dbParameter.Value = value;

            return dbParameter;
        }
    }
}
