using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Function
{
    /// <summary>
    /// 字符串格式化函数类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class StringFormatFunc : ExpressionFunc
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public StringFormatFunc(string name)
            : base(name)
        {
        }

        /// <summary>
        /// 计算函数，将TermValueExpression的参数列表元素相加后回设TermValueExpression
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        public override TermValue Calc(List<TermValue> paramlist)
        {
            string value = "";
            if (paramlist.Count() > 0)
            {
                value = (string)paramlist.First();
                for (int i = 1; i < paramlist.Count(); i++)
                {
                    value += (string)paramlist[i];
                }
            }
            return new TermValue(value);
        }

        public override void InverseCalc(List<TermValue> paramlist, TermValue value)
        {
        }
    }
}
