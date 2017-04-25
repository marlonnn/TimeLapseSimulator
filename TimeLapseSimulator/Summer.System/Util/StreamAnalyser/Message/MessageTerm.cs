using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Protocal;
using Summer.System.Util.StreamAnalyser.Interpreter;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Message
{
    /// <summary>
    /// 协议项解析类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class MessageTerm : MessageTermSlot
    {
        public override string XmlElementName
        {
            get
            {
                return "msgTerm";
            }
        }

        public override long LengthByBit
        {
            get
            {
                if (PT != null)
                {
                    return PT.Interpreter.LengthByBit * (long)PT.Occurs;
                }
                return 0;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="value"></param>
        public MessageTerm(ProtocalTerm pt, TermValue value)
            : base(pt)
        {
            Value = value;            
        }

        public override bool Serialize(byte[] message, ref  InterpreterPosition pos, InterpretOpion io)
        {
            if (PT != null)
            {
                PT.Interpreter.Serialize(message, ref pos, Value, io);
            }
            return true;
        }
    }
}
