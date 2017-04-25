using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Function
{
    /// <summary>
    /// 加法函数类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class AddFunc : ExpressionFunc
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public AddFunc(string name)
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
            long value = 0;
            if (paramlist.Count() > 0)
            {
                value = (long)paramlist.First();
                for (int i = 1; i < paramlist.Count(); i++)
                {
                    value += (long)paramlist[i];
                }
            }
            return new TermValue(value);
        }

        public override void InverseCalc(List<TermValue> paramlist, TermValue value)
        {
            long val = (long)value;
            if ((paramlist.Count() > 0) && paramlist.First().IsVariant())
            {
                for (int i = paramlist.Count() - 1; i > 0; i--)
                {
                    val -= (long)paramlist[i];
                }
                paramlist.First().SetValue(val);
            }
        }
    }

    /// <summary>
    /// 减法函数类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class SubFunc : ExpressionFunc
    {
        public SubFunc(string name)
            : base(name)
        {
        }

        /// <summary>
        /// 计算函数，将TermValueExpression的参数列表元素相减后回设TermValueExpression
        /// 第一个元素为被减数，其余为减数
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        public override TermValue Calc(List<TermValue> paramlist)
        {
            long value = 0;
            if (paramlist.Count() > 0)
            {
                value = (long)paramlist.First();
                for (int i = 1; i < paramlist.Count(); i++)
                {
                    value -= (long)paramlist[i];
                }
            }
            return new TermValue(value);
        }

        public override void InverseCalc(List<TermValue> paramlist, TermValue value)
        {
            long val = (long)value;
            if ((paramlist.Count() > 0) && paramlist.First().IsVariant())
            {
                for (int i = paramlist.Count() - 1; i > 0; i--)
                {
                    val += (long)paramlist[i];
                }
                paramlist.First().SetValue(val);
            }
        }
    }

    /// <summary>
    /// 乘法函数类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class MulFunc : ExpressionFunc
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public MulFunc(string name)
            : base(name)
        {
        }

        /// <summary>
        /// 计算函数，将TermValueExpression的参数列表元素相乘后回设TermValueExpression
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        public override TermValue Calc(List<TermValue> paramlist)
        {
            long value = 0;
            if (paramlist.Count() > 0)
            {
                value = (long)paramlist.First();
                for (int i = 1; i < paramlist.Count(); i++)
                {
                    value *= (long)paramlist[i];
                }
            }
            return new TermValue(value);
        }

        public override void InverseCalc(List<TermValue> paramlist, TermValue value)
        {
            long val = (long)value;
            if ((paramlist.Count() > 0) && paramlist.First().IsVariant())
            {
                for (int i = paramlist.Count() - 1; i > 0; i--)
                {
                    val /= (long)paramlist[i];
                }
                paramlist.First().SetValue(val);
            }
        }
    }

    /// <summary>
    /// 除法函数类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class DivFunc : ExpressionFunc
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public DivFunc(string name)
            : base(name)
        {
        }

        /// <summary>
        /// 计算函数，将TermValueExpression的参数列表元素相除后回设TermValueExpression
        /// 第一个元素为被除数，其余为除数
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        public override TermValue Calc(List<TermValue> paramlist)
        {
            long value = 0;
            if (paramlist.Count() > 0)
            {
                value = (long)paramlist.First();
                for (int i = 1; i < paramlist.Count(); i++)
                {
                    value /= (long)paramlist[i];
                }
            }
            return new TermValue(value);
        }

        public override void InverseCalc(List<TermValue> paramlist, TermValue value)
        {
            long val = (long)value;
            if ((paramlist.Count() > 0) && paramlist.First().IsVariant())
            {
                for (int i = paramlist.Count() - 1; i > 0; i--)
                {
                    val *= (long)paramlist[i];
                }
                paramlist.First().SetValue(val);
            }
        }
    }
}
