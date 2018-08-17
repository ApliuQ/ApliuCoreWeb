using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Apliu.Standard.ORM
{
    public static class ORMHelper
    {
        /// <summary>
        /// 获取对象的Insert语句
        /// </summary>
        /// <param name="modelORM"></param>
        /// <returns></returns>
        public static String GetInsertSql(this IModelORM modelORM)
        {
            TableNameAttribute tableNameAttribute = Attribute.GetCustomAttribute(modelORM.GetType(), typeof(TableNameAttribute)) as TableNameAttribute;
            String tableName = tableNameAttribute?.TableName ?? modelORM.GetType().Name;

            StringBuilder columnSql = new StringBuilder($" insert into {tableName} (");
            StringBuilder valueSql = new StringBuilder($"values (");
            //FieldInfo[] myfields = modelORM.GetType().GetFields();//字段
            PropertyInfo[] propertyInfos = modelORM.GetType().GetProperties();//属性
            foreach (var proInfo in propertyInfos)
            {
                IgnoreAttribute ignoreAttribute = Attribute.GetCustomAttribute(proInfo, typeof(IgnoreAttribute)) as IgnoreAttribute;
                if (!ignoreAttribute?.IsIgnore ?? true)
                {
                    ColumnNameAttribute columnNameAttribute = Attribute.GetCustomAttribute(proInfo, typeof(ColumnNameAttribute)) as ColumnNameAttribute;
                    columnSql.Append(columnNameAttribute?.ColumnName ?? proInfo.Name);
                    columnSql.Append(",");

                    if (proInfo.PropertyType == typeof(String)) valueSql.Append("'" + proInfo.GetValue(modelORM) + "'");
                    else valueSql.Append(proInfo.GetValue(modelORM));
                    valueSql.Append(",");
                }
            }
            columnSql.Remove(columnSql.Length - 1, 1).Append(") ");
            valueSql.Remove(valueSql.Length - 1, 1).Append(")");


            String insertSql = columnSql.ToString() + valueSql.ToString() + ";";
            return insertSql;
        }

        /// <summary>
        /// 获取对象的Update语句
        /// </summary>
        /// <param name="modelORM"></param>
        /// <returns></returns>
        public static String GetUpdateSql(this IModelORM modelORM)
        {
            TableNameAttribute tableNameAttribute = Attribute.GetCustomAttribute(modelORM.GetType(), typeof(TableNameAttribute)) as TableNameAttribute;
            String tableName = tableNameAttribute?.TableName ?? modelORM.GetType().Name;

            StringBuilder columnSql = new StringBuilder($" update {tableName} set ");
            StringBuilder whereSql = null;

            PropertyInfo[] propertyInfos = modelORM.GetType().GetProperties();
            foreach (var proInfo in propertyInfos)
            {
                IgnoreAttribute ignoreAttribute = Attribute.GetCustomAttribute(proInfo, typeof(IgnoreAttribute)) as IgnoreAttribute;
                if (!ignoreAttribute?.IsIgnore ?? true)
                {
                    ColumnNameAttribute columnNameAttribute = Attribute.GetCustomAttribute(proInfo, typeof(ColumnNameAttribute)) as ColumnNameAttribute;
                    IdentityAttribute identityAttribute = Attribute.GetCustomAttribute(proInfo, typeof(IdentityAttribute)) as IdentityAttribute;

                    if (identityAttribute?.IsIdentity ?? false)
                    {
                        if (whereSql == null) whereSql = new StringBuilder(" where ");
                        else whereSql.Append(" and ");
                        whereSql.Append(columnNameAttribute?.ColumnName ?? proInfo.Name);
                        whereSql.Append("=");
                        if (proInfo.PropertyType == typeof(String)) whereSql.Append("'" + proInfo.GetValue(modelORM) + "'");
                        else whereSql.Append(proInfo.GetValue(modelORM));
                    }
                    else
                    {
                        columnSql.Append(columnNameAttribute?.ColumnName ?? proInfo.Name);
                        columnSql.Append("=");
                        if (proInfo.PropertyType == typeof(String)) columnSql.Append("'" + proInfo.GetValue(modelORM) + "'");
                        else columnSql.Append(proInfo.GetValue(modelORM));
                        columnSql.Append(",");
                    }
                }
            }
            columnSql.Remove(columnSql.Length - 1, 1);

            String updateSql = columnSql.ToString() + whereSql?.ToString() + ";";
            return updateSql;
        }

        /// <summary>
        /// 获取对象的删除语句
        /// </summary>
        /// <param name="modelORM"></param>
        /// <returns></returns>
        public static String GetDeleteSql(this IModelORM modelORM)
        {
            TableNameAttribute tableNameAttribute = Attribute.GetCustomAttribute(modelORM.GetType(), typeof(TableNameAttribute)) as TableNameAttribute;
            String tableName = tableNameAttribute?.TableName ?? modelORM.GetType().Name;

            StringBuilder deleteSql = null;

            PropertyInfo[] propertyInfos = modelORM.GetType().GetProperties();
            foreach (var proInfo in propertyInfos)
            {
                IgnoreAttribute ignoreAttribute = Attribute.GetCustomAttribute(proInfo, typeof(IgnoreAttribute)) as IgnoreAttribute;
                if (!ignoreAttribute?.IsIgnore ?? true)
                {
                    ColumnNameAttribute columnNameAttribute = Attribute.GetCustomAttribute(proInfo, typeof(ColumnNameAttribute)) as ColumnNameAttribute;
                    IdentityAttribute identityAttribute = Attribute.GetCustomAttribute(proInfo, typeof(IdentityAttribute)) as IdentityAttribute;

                    if (identityAttribute?.IsIdentity ?? false)
                    {
                        if (deleteSql == null) deleteSql = new StringBuilder($" delete from {tableName} where ");
                        else deleteSql.Append(" and ");
                        deleteSql.Append(columnNameAttribute?.ColumnName ?? proInfo.Name);
                        deleteSql.Append("=");
                        if (proInfo.PropertyType == typeof(String)) deleteSql.Append("'" + proInfo.GetValue(modelORM) + "'");
                        else deleteSql.Append(proInfo.GetValue(modelORM));
                    }
                }
            }

            String updateSql = deleteSql?.ToString() + ";";
            return updateSql;
        }
    }
}
