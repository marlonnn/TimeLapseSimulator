using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summer.System.Data.TypeConversion
{
    /// <summary>
    /// 数据库数据转换器
    /// 将数据库中各个字段的值转成对应的类属性值，如果返回DBNull，则将类属性值设置为default类型得到的值
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-17
    /// </remark>
    public class DBConverterHelper
    {
        private static object syncRoot = new object();
        private static IConvert defaultConverter = new AssignConverter();
        private static readonly IDictionary<Type, IConvert> converterCache = new Dictionary<Type, IConvert>();

        static DBConverterHelper()
        {
            lock(syncRoot)
            {
                converterCache.Add(typeof(float), new FloatConverter());
                converterCache.Add(typeof(int), new IntConverter());
                converterCache.Add(typeof(string), new StringConverter());
                converterCache.Add(typeof(DateTime), new DateTimeConverter());
                converterCache.Add(typeof(bool), new BoolConverter());
            }
        }

        public static IConvert GetConverter(Type type)
        {
            IConvert converter;
            if (!converterCache.TryGetValue(type, out converter))
            {
                converter = defaultConverter;
            }
            return converter;
        }
    }
    /// <summary>
    /// 转换器接口
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-17
    /// </remark>
    public interface IConvert
    {
        /// <summary>
        /// 从一类型转成需要的类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object ConvertFrom(object value);
    }
}
