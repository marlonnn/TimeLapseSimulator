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
    /// 数值类型解析器，以BYTE定义
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-19
    /// </remark>
    class TermSignedInterpreter : TermNumInterpreter
    {
                /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="length"></param>
        public TermSignedInterpreter(TermValue length)
            : base(length)
        {
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="ti"></param>
        public TermSignedInterpreter(TermSignedInterpreter ti)
            : base(ti)
        {
        }

        /// <summary>
        /// 协议解析函数
        /// </summary>
        /// <param name="message">二进制数据流</param>
        /// <param name="index">数据流当前位置</param>
        /// <param name="restbit">当前位置的未解析的Bit数</param>
        /// <param name="value">返回的解析数值类</param>
        /// <param name="io">解析参数类，暂未实现</param>
        /// <returns>成功返回true，否则false</returns>
        public override bool Interpret(byte[] message, ref InterpreterPosition pos, out TermValue value, InterpretOpion io)
        {
            if(base.Interpret(message, ref pos, out value, io))
            {
                long val = (long)value;
                int length = (int)(long)Length;

                if((val & SignedBitMask(length)) != 0)
                {
                    value = new TermValue(SignedValue(val, length));
                }
                return true;
            }
            return false;
        }

        public override TermValue CreateBlank()
        {
            return new TermValue(0);
        }

        /// <summary>
        /// 复制生成TermSignedInterpreter对象
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public override TermInterpreter Clone()
        {
            return new TermSignedInterpreter(this);
        }

        public override void ToXml(XmlVisitor xmlvisitor)
        {
            xmlvisitor.UpdateAttribute("style", TermInterpreter.InterpreterStyle.signed.ToString());
            xmlvisitor.UpdateAttribute("length", Length);
        }

        private long SignedBitMask(int length)
        {
            return 0x80 << ((length - 1) * 8);
        }

        private long SignedValue(long val,int length)
        {
            for (int i = length; i < 8; i++)
            {
                val |= ((long)0xFF) << (i * 8);
            }
            return val;
        }
    }
}
