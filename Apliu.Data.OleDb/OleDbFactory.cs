using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Apliu.Data.OleDb
{
    public static class OleDbFactory
    {
        public static OleDbConnection CreateOleDbConnection(string connectionString)
        {
            OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
            return oleDbConnection;
        }

        public static OleDbCommand CreateOleDbCommand()
        {
            OleDbCommand oleDbCommand = new OleDbCommand();
            return oleDbCommand;
        }

        public static OleDbDataAdapter CreateOleDbDataAdapter(DbCommand oleDbCommand)
        {
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand as OleDbCommand);
            return oleDbDataAdapter;
        }

        public static OleDbParameter CreateOleDbParameter(String parameterName, DbType dbType, Int32 size, Object value)
        {
            OleDbParameter oleDbParameter = new OleDbParameter();
            oleDbParameter.DbType = dbType;
            oleDbParameter.ParameterName = parameterName;
            oleDbParameter.Size = size;
            oleDbParameter.Value = value;
            return oleDbParameter;
        }

        public static OleDbType CreateOleDbType(DbType dbType)
        {
            OleDbType oleDbType = OleDbType.Empty;
            switch (dbType)
            {
                case DbType.AnsiString:
                    oleDbType = OleDbType.VarChar;
                    break;
                case DbType.Binary:
                    oleDbType = OleDbType.VarBinary;
                    break;
                case DbType.Byte:
                    oleDbType = OleDbType.UnsignedTinyInt;
                    break;
                case DbType.Boolean:
                    oleDbType = OleDbType.Boolean;
                    break;
                case DbType.Currency:
                    oleDbType = OleDbType.Currency;
                    break;
                case DbType.Date:
                    oleDbType = OleDbType.Date;
                    break;
                case DbType.DateTime:
                    oleDbType = OleDbType.DBDate;
                    break;
                case DbType.Decimal:
                    oleDbType = OleDbType.Decimal;
                    break;
                case DbType.Double:
                    oleDbType = OleDbType.Double;
                    break;
                case DbType.Guid:
                    oleDbType = OleDbType.Guid;
                    break;
                case DbType.Int16:
                    oleDbType = OleDbType.SmallInt;
                    break;
                case DbType.Int32:
                    oleDbType = OleDbType.Integer;
                    break;
                case DbType.Int64:
                    oleDbType = OleDbType.BigInt;
                    break;
                case DbType.Object:
                    oleDbType = OleDbType.Variant;
                    break;
                case DbType.SByte:
                    oleDbType = OleDbType.TinyInt;
                    break;
                case DbType.Single:
                    oleDbType = OleDbType.Single;
                    break;
                case DbType.String:
                    oleDbType = OleDbType.VarChar;
                    break;
                case DbType.Time:
                    oleDbType = OleDbType.DBTime;
                    break;
                case DbType.UInt16:
                    oleDbType = OleDbType.UnsignedSmallInt;
                    break;
                case DbType.UInt32:
                    oleDbType = OleDbType.UnsignedInt;
                    break;
                case DbType.UInt64:
                    oleDbType = OleDbType.UnsignedBigInt;
                    break;
                case DbType.VarNumeric:
                    oleDbType = OleDbType.VarNumeric;
                    break;
                case DbType.AnsiStringFixedLength:
                    oleDbType = OleDbType.VarChar;
                    break;
                case DbType.StringFixedLength:
                    oleDbType = OleDbType.VarWChar;
                    break;
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    oleDbType = OleDbType.DBTimeStamp;
                    break;
                case DbType.Xml:
                default:
                    oleDbType = OleDbType.Empty;
                    break;
            }
            return oleDbType;
        }
    }
}
