using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Interpreter;
using Summer.System.Util.StreamAnalyser.Protocal;
using Summer.System.Util.StreamAnalyser.Value;
using Summer.System.IO;

namespace Summer.System.Util.StreamAnalyser.Message
{
    /// <summary>
    /// 复杂协议项解析类，包含多个基本协议解析类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class MessageTermComplex : MessageTerm
    {
        public override string XmlElementName
        {
            get
            {
                return "msgTermCpx";
            }
        }

        /// <summary>
        /// 基本协议解析类，用来保存复杂的协议解析信息
        /// </summary>
        public List<MessageTermSlot> MessageTermList { get; protected set; }

        public override long LengthByBit
        { 
            get
            {
                long lengthbybit = 0;
                if (PT != null)
                {
                    lengthbybit = base.LengthByBit;
                }
                if (0 == lengthbybit)
                {
                    foreach (MessageTermSlot mt in MessageTermList)
                    {
                        lengthbybit += mt.LengthByBit;
                    }
                }
                return lengthbybit; 
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="value"></param>
        /// <param name="mtList"></param>
        public MessageTermComplex(ProtocalTerm pt, TermValue value, List<MessageTermSlot> mtList)
            : base(pt, value)
        {
            MessageTermList = new List<MessageTermSlot>(mtList);
        }

        public override bool Serialize(byte[] message, ref InterpreterPosition pos, InterpretOpion io)
        {
            if ((PT != null) && !(PT.Interpreter is TermNoneInterpreter))
            {
                PT.Interpreter.Serialize(message, ref pos, Value, io);
            }
            else
            {
                foreach (MessageTermSlot mt in MessageTermList)
                {
                    mt.Serialize(message, ref pos, io);
                }
            }
            return true;
        }

        public override XmlVisitor ToXml()
        {
            XmlVisitor xmlTerm = base.ToXml();
            foreach (MessageTermSlot mt in MessageTermList)
            {
                xmlTerm.AppendChild(mt.ToXml());
            }
            return xmlTerm;
        }

        public override bool ModifyTerm(MessageTermSlot mt, MessageStream ms, InterpretOpion io)
        {
            int index = MessageTermList.FindIndex(r => r.Name == mt.Name);
            if (index != -1)
            {
                MessageTermList[index] = mt;
                ProtocalTermComplex ptc = PT as ProtocalTermComplex;
                if ((ptc != null) && (ptc.PTList is ProtocalTermSequence) && !(PT.Interpreter is TermNoneInterpreter))
                {
                    byte[] message = new byte[LengthByBit];
                    InterpreterPosition pos = new InterpreterPosition();
                    foreach (MessageTermSlot mts in MessageTermList)
                    {
                        mts.Serialize(message, ref pos, io);
                    }
                    pos = new InterpreterPosition();
                    TermValue value = null;
                    if (PT.Interpreter.Interpret(message, ref pos, out value, io))
                    {
                        UpdateRelateTerm(PT.Name, value, ms, io);
                    }else
                    {
	                    var log= Interpreter.InterpretLog.GetInstance();
	                    log.FullName = PT.FullName;
	                    log.Pos = pos;
                    }
                }
            }
            return true;
        }
    }
}
