using System;
using System.Collections.Generic;
using System.Text;

namespace Apliu.Standard.ORM
{
    /// <summary>
    /// ORM Model对象 属性中必须存在特性Identity,且必须存在get函数
    /// 注意区分字段和属性的区别
    /// </summary>
    public interface IModelORM { }

    /// <summary>
    /// 测试Model
    /// </summary>
    [TableName("ModelClass")]
    public class ModelClass : IModelORM
    {
        [Identity(true)]
        [ColumnName("Id")]
        public String Id { get; set; }
        [ColumnName("Name")]
        public String Name { get; set; }
        [ColumnName("Count")]
        public Int32 Count { get; set; }
        [Ignore(true)]
        public String Remark { get; set; }
    }
}
