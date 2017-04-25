using System;

namespace Summer.System.Data
{
    /// <summary>
    /// Summer库持久层相关异常，主要包括各种数据访问异常
    /// <para>查看innerException知晓详细信息</para>
    /// </summary>
    /// <remarks>
    /// 公司：CASCO
    /// 作者：戴唯艺                 
    /// 创建日期：2013-5-23   
    /// </remarks>
    class DatabaseException : Exception
    {
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
