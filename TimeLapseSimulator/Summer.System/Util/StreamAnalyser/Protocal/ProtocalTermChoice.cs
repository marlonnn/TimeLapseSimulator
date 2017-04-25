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
    /// 选择协议项组类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class ProtocalTermChoice : ProtocalTermList
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xmlVisitor"></param>
        /// <param name="pt"></param>
        public ProtocalTermChoice(XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm pt = null)
            : base(xmlVisitor, xmlAliasList, pt)
        {
            if (xmlVisitor.Name == "choice")
            {
                foreach (XmlVisitor xmlChild in xmlVisitor.FilterChildren("data"))
                {
                    Value.AddRange(ProtocalTerm.Create(xmlChild, xmlAliasList, pt));
                }
            }
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="ptlist"></param>
        public ProtocalTermChoice(ProtocalTermChoice ptlist)
            : base(ptlist)
        {
        }

        /// <summary>
        /// 协议解析函数
        /// </summary>
        /// <param name="message">二进制数据流</param>
        /// <param name="index">数据流当前位置</param>
        /// <param name="restbit">当前位置的未解析的Bit数</param>
        /// <param name="end">解析完后是否需要结束解析过程</param>
        /// <param name="ti">解析器对象</param>
        /// <param name="value">父级协议解析类解析出的数据</param>
        /// <param name="io">解析参数类，暂未实现</param>
        /// <returns>返回解析结果</returns>
        public override List<MessageTermSlot> Interpret(byte[] message, ref InterpreterPosition pos, ref bool end, TermInterpreter ti, TermValue value, InterpretOpion io)
        {
            InterpreterPosition pos2 = new InterpreterPosition(pos);
            List<MessageTermSlot> mtList = new List<MessageTermSlot>();
            foreach (ProtocalTerm pt in Value)
            {
                pos = new InterpreterPosition(pos2);

                MessageTermSlot mt = null;
                if (pt is ProtocalTermFramer)
                {//如果该解析项为Choice_Frame引用，则
                    ProtocalTermFramer ptf = pt as ProtocalTermFramer;
                    mt = ptf.Interpret(message, ref pos, io);
                    if (mt != null)
                    {
                        end = ptf.End;
                        mtList.Add(mt);
                        break;
                    }
                }
                else if (pt is ProtocalTermComplex)
                {//如果该解析项为复合项，则
                }
                else 
                {//如果该解析项为一般项，则
                    if (ti.IsMatch(pt.Interpreter))
                    {//如果传入的解析项(ti)类型和当前的解析项类型pt.Interpreter相同
						//常用在通过Choice选择Meaning的情况
                        if (value.Equals(pt.Value))
                        {
                            mt = ProtocalTerm.CreateMessageTerm(pt, value);
                        }
                    }
                    else
                    {
                        mt = pt.Interpret(message, ref pos, io);
                        if ((mt != null) && !pt.Value.Equals(mt.Value))
                        {
                            mt = null;
                        }
                    }
                    if (mt != null)
                    {//如果确实生成了msgTerm，则把新生成的msgTerm放入List中，然后退出查找
                        end = pt.End;
                        mtList.Add(mt);
                        break;
                    }
                }
            }
            if (mtList.Count == 0)//未匹配任何一个协议定义项，建议结束解析过程
            {
                end = true;
            }
            return mtList;
        }

        public override List<MessageTermSlot> Transfer(XmlVisitor xmlVisitor, TermInterpreter ti, TermValue value)
        {
            List<MessageTermSlot> mtList = new List<MessageTermSlot>();
            foreach (ProtocalTerm pt in Value)
            {
                MessageTermSlot mt = null;
                if (pt is ProtocalTermFramer)
                {
                    ProtocalTermFramer ptf = pt as ProtocalTermFramer;
                    mt = ptf.Transfer(xmlVisitor);
                    if (mt != null)
                    {
                        mtList.Add(mt);
                        break;
                    }
                }
                else if (pt is ProtocalTermComplex)
                {

                }
                else
                {
                    ProtocalTerm pts = pt as ProtocalTerm;
                    if (ti.IsMatch(pt.Interpreter))
                    {
                        if (value.Equals(pts.Value))
                        {
                            mt = ProtocalTerm.CreateMessageTerm(pt, value);
                        }
                    }
                    else
                    {
                        mt = pts.Transfer(xmlVisitor);
                        if ((mt != null) && !pts.Value.Equals(mt.Value))
                        {
                            mt = null;
                        }
                    }
                    if (mt != null)
                    {
                        mtList.Add(mt);
                        break;
                    }
                }
            }
            return mtList;
        }

        public override List<MessageTermSlot> CreateBlank(TermInterpreter ti, TermValue value)
        {
            List<MessageTermSlot> mtList = new List<MessageTermSlot>();
            foreach (ProtocalTerm pt in Value)
            {
                MessageTermSlot mt = null;
                if (pt is ProtocalTermFramer)
                {
                }
                else if (pt is ProtocalTermComplex)
                {

                }
				else
                {                    
                    if (ti.IsMatch(pt.Interpreter))
                    {
                        if (value.Equals(pt.Value))
                        {
                            mt = pt.CreateBlank();
                        }
                    }
                    if (mt != null)
                    {
                        mtList.Add(mt);
                        break;
                    }
                }
            }
            return mtList;
        }

        public override List<MessageTermSlot> CreateTerm(byte[] message, ProtocalTermComplex ptc, MessageTermSlot value, InterpretOpion io)
        {
            List<MessageTermSlot> mtList = new List<MessageTermSlot>();
            foreach (ProtocalTerm pt in Value)
            {
                InterpreterPosition pos = new InterpreterPosition() ;

                MessageTermSlot mt = null;
                if (pt is ProtocalTermFramer)
                {
                    ProtocalTermFramer ptf = pt as ProtocalTermFramer;
                    mt = ptf.CreateTerm(value, io);
                    if (mt != null)
                    {
                        mtList.Add(mt);
                        break;
                    }
                }
                else if(pt is ProtocalTermComplex)
                {

                }
                else
                {
                    if (ptc.Interpreter.IsMatch(pt.Interpreter))
                    {
                        if (value.Value.Equals(pt.Value))
                        {
                            mt = ProtocalTerm.CreateMessageTerm(pt, value.Value);
                        }
                    }
                    else
                    {
                        mt = pt.Interpret(message, ref pos, io);
                        if ((mt != null) && !pt.Value.Equals(mt.Value))
                        {
                            mt = null;
                        }
                    }
                    if (mt != null)
                    {
                        mtList.Add(mt);
                        break;
                    }
                }
            }
            return mtList;
        }

        public override bool Vertify(MessageTermSlot mt)
        {
            foreach (ProtocalTerm pt in Value)
            {
                if (pt.Value.Equals(mt.Value))
                {
                    return pt.Verify(mt);
                }
            }
            return false;
        }

        /// <summary>
        /// 复制生成ProtocalTermList及其子类型对象
        /// </summary>
        /// <param name="ptlist"></param>
        /// <returns></returns>
        public override ProtocalTermList Clone()
        {
            return new ProtocalTermChoice(this);
        }
    }
}
