using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Function
{
    /// <summary>
    /// 枚举映射函数类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class EnumFunc : ExpressionFunc
    {
        /// <summary>
        /// 枚举关系字典
        /// </summary>
        Dictionary<string, TermValue> EnumValueList;

        /// <summary>
        /// 缺省数值
        /// </summary>
        TermValue defaultValue;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enumvaluelist"></param>
        public EnumFunc(string name, Dictionary<string, TermValue> enumvaluelist, string dftValue)
            : base(name)
        {
            EnumValueList = enumvaluelist;
            defaultValue = new TermValue(dftValue);
        }

        /// <summary>
        /// 计算函数，将TermValueExpression的参数列表第一元素根据枚举关系字典转换后回设TermValueExpression
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        public override TermValue Calc(List<TermValue> paramlist)
        {
            if ((paramlist.Count() > 0) && EnumValueList.ContainsKey(paramlist.First().value))
            {
                return EnumValueList[paramlist.First().value];
            }
            return defaultValue;
        }

        public override void InverseCalc(List<TermValue> paramlist, TermValue value)
        {
            long val = (long)value;
            if (paramlist.Count() > 0)
            {
                foreach (var kvp in EnumValueList)
                {
                    if (kvp.Value.Equals(value))
                    {
                        paramlist.First().SetValue(kvp.Key);
                    }
                }                
            }
        }
    }
}
