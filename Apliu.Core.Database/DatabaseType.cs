using System;

namespace Apliu.Core.Database
{
    public enum DatabaseType
    {
        SqlServer = 0,
        Oracle = 1,
        MySql = 2,
        /// <summary>
        /// Microsoft Access 数据库(.accdb)
        /// </summary>
        [Obsolete]
        OleDb = 3
    }
}
