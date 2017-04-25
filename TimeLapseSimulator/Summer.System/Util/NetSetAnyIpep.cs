using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Summer.System.Util
{
    /// <summary>
    /// 利用IPEndPoint类，设置IP地址和端口号为本机的任意IP地址和端口号
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：田绪俊
    /// 创建日期：2013-5-16
    /// </remark>
    class NetSetAnyIpep
    {
        /// <summary>
        /// 设置结点IP地址和端口号，可以为本机的任意IP地址和端口号
        /// </summary>
        /// <returns></returns>
        public static IPEndPoint SetAnyIpep()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
            return ipep;
        }
    }
}
