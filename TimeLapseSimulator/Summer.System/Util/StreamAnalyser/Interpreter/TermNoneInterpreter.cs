using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Value;
using Summer.System.Util.StreamAnalyser.Protocal;
using Summer.System.IO;

namespace Summer.System.Util.StreamAnalyser.Interpreter
{
    /// <summary>
    /// 无类型解析器
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class TermNoneInterpreter : TermInterpreter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="length"></param>
        public TermNoneInterpreter()
            : base(new TermValue(0))
        {
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="ti"></param>
        public TermNoneInterpreter(TermNoneInterpreter ti)
            : base(ti)
        {
        }

        /// <summary>
        /// 协议解析函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="restbit"></param>
        /// <param name="value"></param>
        /// <param name="io"></param>
        /// <returns></returns>
        public override bool Interpret(byte[] message, ref InterpreterPosition pos, out TermValue value, InterpretOpion io)
        {
            value = new TermValue("");
            return false;
        }

        /// <summary>
        /// 序列化协议对象
        /// </summary>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="restbit"></param>
        /// <param name="value"></param>
        /// <param name="io"></param>
        /// <returns></returns>
        public override bool Serialize(byte[] message, ref InterpreterPosition pos, TermValue value, InterpretOpion io)
        {
            return false;
        }

        public override TermValue CreateBlank()
        {
            return new TermValue("");
        }

        /// <summary>
        /// 复制生成TermNoneInterpreter对象
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public override TermInterpreter Clone()
        {
            return new TermNoneInterpreter(this);
        }

        public override void ToXml(XmlVisitor xmlvisitor)
        {
        }
    }
}
