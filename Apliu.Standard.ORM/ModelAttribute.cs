using System;
using System.Collections.Generic;
using System.Text;

namespace Apliu.Standard.ORM
{
    /// <summary>
    /// 表名
    /// </summary>
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public String TableName { get; set; }

        public TableNameAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }

    /// <summary>
    /// 列名
    /// </summary>
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// 列名
        /// </summary>
        public String ColumnName { get; set; }

        public ColumnNameAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }
    }

    /// <summary>
    /// 是否是主键
    /// </summary>
    public class IdentityAttribute : Attribute
    {
        /// <summary>
        /// 是否是主键
        /// </summary>
        public Boolean IsIdentity { get; set; } = false;
        public IdentityAttribute(bool isIdentity)
        {
            this.IsIdentity = isIdentity;
        }
    }

    /// <summary>
    /// 是否忽略该属性
    /// </summary>
    public class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// 是否忽略该属性
        /// </summary>
        public Boolean IsIgnore { get; set; } = false;
        public IgnoreAttribute(bool isIgnore)
        {
            this.IsIgnore = isIgnore;
        }
    }
}
