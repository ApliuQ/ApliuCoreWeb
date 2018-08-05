using System;

namespace ApliuCoreDatabase
{
    public enum DatabaseType
    {
        SqlServer = 0,
        Oracle = 1,
        MySql = 2,
        /// <summary>
        /// Microsoft Access 数据库(.accdb)
        /// </summary>
        OleDb = 3
    }
}
