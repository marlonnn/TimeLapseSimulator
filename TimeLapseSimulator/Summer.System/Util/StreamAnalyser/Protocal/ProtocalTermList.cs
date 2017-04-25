using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Message;
using Summer.System.Util.StreamAnalyser.Interpreter;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Protocal
{
    /// <summary>
    /// 协议项组基类，为虚基类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public abstract class ProtocalTermList
    {
        /// <summary>
        /// 协议项组列表
        /// </summary>
        public List<ProtocalTerm> Value { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProtocalTermList(XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm pt = null)
        {
            Value = new List<ProtocalTerm>();
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="ptlist"></param>
        public ProtocalTermList(ProtocalTermList ptlist)
        {
            Value = new List<ProtocalTerm>();
            foreach (ProtocalTerm pt in ptlist.Value)
            {
                Value.Add(pt.IsVariant() ? pt.Clone() : pt);
            }
        }

        /// <summary>
        /// 协议解析函数,在子类实现
        /// </summary>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="restbit"></param>
        /// <param name="end"></param>
        /// <param name="ti"></param>
        /// <param name="value"></param>
        /// <param name="io"></param>
        /// <returns></returns>
        public abstract List<MessageTermSlot> Interpret(byte[] message, ref InterpreterPosition pos, ref bool end, TermInterpreter ti, TermValue value, InterpretOpion io);

        public abstract List<MessageTermSlot> Transfer(XmlVisitor xmlVisitor, TermInterpreter ti, TermValue value);

        public abstract List<MessageTermSlot> CreateBlank(TermInterpreter ti, TermValue value);

        public abstract List<MessageTermSlot> CreateTerm(byte[] message, ProtocalTermComplex ptc, MessageTermSlot value, InterpretOpion io);

        public abstract bool Vertify(MessageTermSlot mt);

        /// <summary>
        /// 整合函数，用以关联协议的索引，在协议类全部生成后调用
        /// </summary>
        /// <param name="pd"></param>
        public virtual void Intergation(ProtocalDesigner pd)
        {
            foreach (ProtocalTerm pt in Value)
            {
                pt.Intergation(pd);
            }
        }

        /// <summary>
        /// 是否为变量
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsVariant()
        {
            foreach (ProtocalTerm pt in Value)
            {
                if (pt.IsVariant())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 复制生成ProtocalTermList及其子类型对象
        /// </summary>
        /// <param name="ptlist"></param>
        /// <returns></returns>
        public virtual ProtocalTermList Clone()
        {
            return null;
        }
    }
}
