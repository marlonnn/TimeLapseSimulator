using System;
using System.Collections.Generic;
using System.Reflection;
using Summer.System.Data.TypeConversion;
using Summer.System.Reflection.Dynamic;

namespace Summer.System.Data.DbMapping
{
    /// <summary>
    /// 把一行数据转成对象实体,注意：是填充Property，而不是Field
    /// </summary>
    /// <remarks>
    /// 公司：CASCO
    /// 作者：张立鹏                 
    /// 创建日期：2013-5-17   
    /// </remarks>
    /// <remarks>
    /// 修改人： 戴唯艺，日期：2-13-5-22
    /// 修改内容：
    /// 1, 转化为泛型类; 
    /// 2, 规定只能填充Class.Property而不是Class.Fields
    /// 3, 确保reader的Index引用为Database字段名，而不是Property Name
    /// 4. 添加DBNull防护
    /// </remarks>
	public class RowMapper<T> : Spring.Data.Generic.IRowMapper<T>
    {
        public T MapRow(global::System.Data.IDataReader reader, int rowNum)
        {
            //构造一个对象实例
            var sc = new SafeConstructor(typeof(T).GetConstructor(new Type[0]));
            //为了此函数能顺利进行，T类型必须得有一个空构造函数
            object o = sc.Invoke(new object[0]);

            Dictionary<string, FieldAttribute> dbFieldDef = MappingHelper.Get_PropertyName_DbFieldDef(typeof(T));

            //将reader中的值赋走
            foreach (var def in dbFieldDef)
            {
                FieldInfo proInfo = typeof(T).GetField(def.Key);
                var sp = new SafeField(proInfo);
                //确保Reader的index引用为Database 列名，而不是Class的Property Name，有些情况这2值不相同
                object value = reader[def.Value.Name];
                if (!Convert.IsDBNull(value))
                {
                    //数据库空Cell取值不会取出Null值，只有DBNull类型的值,
                    sp.SetValue(o,
                                DBConverterHelper.GetConverter(proInfo.FieldType).ConvertFrom(value));
                }
            }

            return o is T ? (T)o : default(T);
        }
    }
}
