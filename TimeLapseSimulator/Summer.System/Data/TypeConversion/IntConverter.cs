using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Summer.System.Data.TypeConversion
{
    /// <summary>
    /// Int类型转换类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-17
    /// </remark>
    public class IntConverter : IConvert
    {
        public object ConvertFrom(object value)
        {
            object newValue;

            //如果值是DBNull类型，则返回default
            if (value is DBNull)
            {
                return default(int);
            }

            if (value.GetType().Equals(typeof(Int16))
                || value.GetType().Equals(typeof(Int32)))
                return value;
            //将long类型转成int
            if (value.GetType().Equals(typeof(long))
                || value.GetType().Equals(typeof(Int64)))
            {
                long d = (long)value;
                newValue = (int)d;
            }
            else
            {
                throw new Exception(string.Format("不能从类型{0}转化为Int类型。", value.GetType().ToString()));
            }
            return newValue;
        }
    }
}
