using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Function;
using Summer.System.Util.StreamAnalyser.Protocal;

namespace Summer.System.Util.StreamAnalyser.Value
{
    /// <summary>
    /// 表达式数据值类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class TermValueExpression : TermValue
    {
        /// <summary>
        /// 表达式函数名称
        /// </summary>
        public string FuncName { get; protected set; }

        /// <summary>
        /// 表达式函数类实例，在Intergation函数中设置
        /// </summary>
        protected ExpressionFunc func { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<TermValue> paramlist { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="val"></param>
        /// <param name="plst"></param>
        public TermValueExpression(string fname, TermValue val, List<TermValue> plst)
            : base(val)
        {
            FuncName = fname;
            paramlist = plst;
            func = null;
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="tve"></param>
        public TermValueExpression(TermValueExpression tve)
            : base(tve)
        {
            FuncName = tve.FuncName;
            paramlist = new List<TermValue>(tve.paramlist);
            func = tve.func;
        }

        public override void SetValue(long val)
        {
            base.SetValue(val);
            if (IsVariant())
            {
                if ((null != func) && (null != paramlist))
                {
                    func.InverseCalc(paramlist,new TermValue(this));
                }
            }
        }

        /// <summary>
        /// 根据参数类表决定是否是变量类型
        /// </summary>
        /// <returns></returns>
        public override bool IsVariant()
        {
            foreach (TermValue param in paramlist)
            {
                if (param.IsVariant())
                {
                    return true;
                }
            }
            return false;
        }

        public override TermValueRef Variant()
        {
            foreach (TermValue param in paramlist)
            {
                if (param is TermValueRef)
                {
                    return param.Variant() ;
                }
                else if (param is TermValueExpression)
                {
                    if (param.IsVariant())
                    {
                        return param.Variant();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据表达式函数赋值运算
        /// </summary>
        protected override void Assign()
        {
            if ((null != func) && (null != paramlist))
            {
                SetValue(func.Calc(paramlist));
            }
        }

        /// <summary>
        /// 复制生成TermValue对象
        /// </summary>
        /// <returns></returns>
        public override TermValue Clone()
        {
            return new TermValueExpression(this);
        }

        /// <summary>
        /// 整合函数，关联函数及参数列表（如需要）至至协议相关项
        /// </summary>
        /// <param name="eflist"></param>
        /// <param name="pt"></param>
        public override void Intergation(List<ExpressionFunc> eflist, ProtocalTerm pt)
        {
            if ((!string.IsNullOrEmpty(FuncName)) && (pt != null))
            {
                func = eflist.Find(r => r.Name == FuncName);
                foreach (TermValue param in paramlist)
                {
                    if (param is TermValueRef)
                    {
                        (param as TermValueRef).Intergation(eflist, pt);
                    }
                }
            }
        }
    }
}
