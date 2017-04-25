using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Protocal;
using Summer.System.Util.StreamAnalyser.Function;

namespace Summer.System.Util.StreamAnalyser.Value
{
    /// <summary>
    /// 变量数据值类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class TermValueRef : TermValue
    {
        /// <summary>
        /// 以字符串形式保存数据值
        /// </summary>
        public string ValueRef { get; protected set; }

        /// <summary>
        /// 根据字符串生成数据项值类
        /// </summary>
        /// <param name="val"></param>
        public TermValueRef(string valref)
            : base("")
        {
            ValueRef = valref;
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="tvr"></param>
        public TermValueRef(TermValueRef tvr)
            : base(tvr)
        {
            ValueRef = tvr.ValueRef;
        }

        /// <summary>
        /// 变量类型，恒为true
        /// </summary>
        /// <returns></returns>
        public override bool IsVariant()
        {
            return true;
        }

        public override TermValueRef Variant()
        {
            return this;
        }

        /// <summary>
        /// 复制生成TermValue对象
        /// </summary>
        /// <returns></returns>
        public override TermValue Clone()
        {
            return new TermValueRef(this);
        }

        /// <summary>
        /// 整合函数，关联变量至协议相关项
        /// </summary>
        /// <param name="eflist"></param>
        /// <param name="pt"></param>
        public override void Intergation(List<ExpressionFunc> eflist, ProtocalTerm pt)
        {
            ProtocalTerm parentPT = pt;
            while (parentPT != null)
            {
                if(parentPT.AddRelateTerm(ValueRef, this))
                {
                    break;
                }
                parentPT = parentPT.parentPT;
            }
        }

		
    }
}
