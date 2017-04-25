using System;

namespace Summer.System.Data.DbMapping
{
    /// <summary>
    /// 对Class设置属性，使之自动和数据库表相关联
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：戴唯艺                 
    /// 创建日期：2013-5-23   
    /// </remark>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TableAttribute : Attribute
    {
        public TableAttribute(string name)
        {
	        Name = name;
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get;  set; }
    }
}
