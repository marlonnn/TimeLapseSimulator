using Spring.Context;
using Spring.Context.Support;

namespace Summer.System.Core
{
    /// <summary>
    /// Spring的简单封装类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-4-21
    /// </remark>
    public class SpringHelper
    {
        private static IApplicationContext context = GetContext();

        public static IApplicationContext GetContext()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            return ctx;
        }

        /// <summary>
        /// 根据配置名称得到实例
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="name">配置文件中的object id值</param>
        /// <returns></returns>
        public static T GetObject<T>(string name)
        {
            return context.GetObject<T>(name);
        }
    }
}
