using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Summer.System.Data.TypeConversion
{
    /// <summary>
    /// String类型转换类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-17
    /// </remark>
    public class StringConverter : IConvert
    {
        public object ConvertFrom(object value)
        {
            object newValue;

            //如果值是DBNull类型，则返回default
            if (value is DBNull)
            {
                return default(string);
            }

            if (value.GetType().Equals(typeof(string)))
                return value;

            newValue = value.ToString();
            return newValue;
        }
    }
}
