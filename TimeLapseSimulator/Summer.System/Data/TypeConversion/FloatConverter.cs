using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Summer.System.Data.TypeConversion
{
    /// <summary>
    /// Float类型转换类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-17
    /// </remark>
    public class FloatConverter : IConvert
    {
        public object ConvertFrom(object value)
        {
            object newValue;
            //如果值是DBNull类型，则返回default
            if (value is DBNull)
            {
                return default(float);
            }
            //如果源类型也是float，则原值返回
            if (value.GetType().Equals(typeof(float)))
            {
                return value;
            }
            //将double类型转成float
            if (value.GetType().Equals(typeof(double))
                || value.GetType().Equals(typeof(Double)))
            {
                double d = (double)value;
                newValue = (float)d;
            }
            if (value.GetType().Equals(typeof(string)))
            {
                string s = (string)value;
                newValue = float.Parse(s);
            }
            else
            {
                throw new Exception(string.Format("不能从类型{0}转化为Float类型。", value.GetType().ToString()));
            }
            return newValue;
        }
    }
}
