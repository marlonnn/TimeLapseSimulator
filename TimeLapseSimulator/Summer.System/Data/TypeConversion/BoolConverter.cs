using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Summer.System.Data.TypeConversion
{
    /// <summary>
    /// Bool类型转换类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2015-5-14
    /// </remark>
    public class BoolConverter : IConvert
    {
        public object ConvertFrom(object value)
        {
            object newValue;

            //如果值是DBNull类型，则返回default
            if (value is DBNull)
            {
                return default(bool);
            }

            if (value.GetType().Equals(typeof(bool)))
                return value;
            //将字符串类型转成int
            if (value.GetType().Equals(typeof(string)))
            {
                string v = value as string;
                v = v.ToLower();
                if (v.Equals("true") || v.Equals("1"))
                {
                    newValue = true;
                }
                else
                {
                    newValue = false;
                }
            }
            else
            {
                throw new Exception(string.Format("不能从类型{0}转化为bool类型。", value.GetType().ToString()));
            }
            return newValue;
        }
    }
}
