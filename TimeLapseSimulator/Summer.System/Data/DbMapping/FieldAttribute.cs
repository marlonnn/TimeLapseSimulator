using System;

namespace Summer.System.Data.DbMapping
{
    /// <summary>
    /// 对类对象Property设置属性，使之自动和数据库字段相关联
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：戴唯艺                 
    /// 创建日期：2013-5-23   
    /// </remark>
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed class FieldAttribute : Attribute
    {
        public FieldAttribute(string name)
        {
	        Name = name;
            PrimaryKey = false;
        }
        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 主键标识
        /// </summary>
        public bool PrimaryKey { get; set; }

		public bool AutoIncrea { get; set; }
    }


}