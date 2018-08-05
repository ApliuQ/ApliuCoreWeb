using Apliu.Data.OleDb;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
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
        public string databaseConnection
        {
            // Data Source={0};Initial Catalog={1};Integrated Security=False;User ID={2};Password={3};Connect Timeout=15;Encrypt=False;TrustServerCertificate=False
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
        public DatabaseHelper(String ConnectionString)
        {
            this._databaseConnection = ConnectionString;
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
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间
            transactionOption.Timeout = new TimeSpan(0, 0, seconds);
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
        /// 执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="commandText">Sql语句或存储过程</param>
        /// <param name="commandTimeout">语句执行的超时时间</param>
        /// <param name="commandParameters">语句参数 </param>
        /// <returns>返回受影响的行数</returns>
        public int PostDataExecute(CommandType commandType, string commandText, int commandTimeout, params object[] commandParameters)
        {
            int val = -1;
            try
            {
                using (DbConnection dbConnection = CreateDbConnection(databaseConnection))
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
                        val = dbCommand.ExecuteNonQuery();
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                val = -1;
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

            return val;
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
                using (DbConnection dbConnection = CreateDbConnection(databaseConnection))
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
                case DatabaseType.Oracle:
                case DatabaseType.MySql:
                case DatabaseType.OleDb:
                    databaseConnectionStr = beginConnectionStr + ";"
                                    + "Max Pool Size=" + MaxPool + ";"
                                    + "Min Pool Size=" + MinPool + ";"
                                    + "Connect Timeout=" + Conn_Timeout + ";"
                                    + "Connection Lifetime=" + Conn_Lifetime + ";";
                    break;
                default:
                    throw new Exception("数据库类型有误或未初始化 databaseType：" + databaseType.ToString());
                    break;
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
            //System.Data.OleDb.OleDbPermissionAttribute
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
                    break;
                default:
                    throw new Exception("数据库类型有误或未初始化，Database Type：" + databaseType.ToString());
                    break;
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
                    break;
                default:
                    throw new Exception("数据库类型有误或未初始化 databaseType：" + databaseType.ToString());
                    break;
            }

            return dbDataAdapter;
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
                    ((OracleParameter)dbParameter).OracleType = (OracleType)Enum.Parse(typeof(OracleType), dbType);
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
                    break;
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
