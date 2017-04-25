using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summer.System.Util
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-31
    /// </remark>
    public class DbHelper
    {
        /// <summary>
        /// 主键产生器，前17位为时间（4位年 2位月 2位日 2位时 2位分 2位秒 3位毫秒），后8位为随机数，共计25位
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            DateTime dt = DateTime.Now;
            string key = string.Format("{0:yyyyMMddHHmmssfff}{1}", dt, Guid.NewGuid().ToString().Substring(0, 8));
            return key;
        }
    }
}
