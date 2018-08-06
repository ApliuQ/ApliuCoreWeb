using System;
using System.Data;
using System.Data.Common;

namespace Apliu.Core.Database
{
    [Obsolete]
    public class DatabaseParameter : DbParameter
    {
        public override DbType DbType { get; set; }
        public override ParameterDirection Direction { get; set; }
        public override bool IsNullable { get; set; }
        public override string ParameterName { get; set; }
        public override int Size { get; set; }
        public override string SourceColumn { get; set; }
        public override bool SourceColumnNullMapping { get; set; }
        public override object Value { get; set; }

        public DatabaseParameter() : base()
        {
            this.ParameterName = String.Empty;
            this.DbType = DbType.Single;
            this.Size = 0;
            this.Direction = ParameterDirection.Input;
            this.IsNullable = false;
            this.SourceColumn = String.Empty;
            this.Value = null;
            this.SourceColumnNullMapping = false;
        }

        public override void ResetDbType()
        {
        }
    }
}
