using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summer.System.Data.TypeConversion
{
    /// <summary>
    /// 直接赋值转换
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-17
    /// </remark>
    public class AssignConverter : IConvert
    {
        public object ConvertFrom(object value)
        {
            if (value is DBNull)
            {
                return null;
            }
            return value;
        }
    }
}
