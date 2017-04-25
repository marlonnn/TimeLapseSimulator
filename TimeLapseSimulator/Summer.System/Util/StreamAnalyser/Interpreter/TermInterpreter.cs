using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Value;
using Summer.System.Util.StreamAnalyser.Protocal;

namespace Summer.System.Util.StreamAnalyser.Interpreter
{
    /// <summary>
    /// 解析类虚基类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public abstract class TermInterpreter
    {
        public enum InterpreterStyle
        {
            none,//0
            bit,//1
            number,//2
            signed,//3
            bytes,//4
            str,//5
        };

        /// <summary>
        /// 需解析的数据长度
        /// </summary>
        public TermValue Length { get; protected set; }

        public virtual long LengthByBit
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="length"></param>
        protected TermInterpreter(TermValue length)
        {
            Length = length;
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="ti"></param>
        protected TermInterpreter(TermInterpreter ti)
        {
            if (ti.Length.IsVariant())
            {
                Length = ti.Length.Clone();
            }
            else
            {
                Length = ti.Length;
            }
        }

        /// <summary>
        /// 协议解析函数,在子类实现
        /// </summary>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="restbit"></param>
        /// <param name="data"></param>
        /// <param name="io"></param>
        /// <returns></returns>
        public abstract bool Interpret(byte[] message, ref InterpreterPosition pos, out TermValue data, InterpretOpion io);

        /// <summary>
        /// 序列化协议对象
        /// </summary>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="restbit"></param>
        /// <param name="value"></param>
        /// <param name="io"></param>
        /// <returns></returns>
        public virtual bool Serialize(byte[] message, ref InterpreterPosition pos, TermValue value, InterpretOpion io)
        {
            return false;
        }

        /// <summary>
        /// 判断两种解析器在解析动作是否一致
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public virtual bool IsMatch(TermInterpreter ti)
        {
            if (ti is TermNoneInterpreter)
            {
                return false;
            }
            return ((ti.GetType() == GetType()) && (ti.LengthByBit == LengthByBit));
        }

        /// <summary>
        /// 是否为变量
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsVariant()
        {
            if (Length.IsVariant())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 复制生成TermInterprete对象
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public abstract TermInterpreter Clone();

        public abstract void ToXml(XmlVisitor xmlvisitor);

        public abstract TermValue CreateBlank();


        /// <summary>
        /// 根据参数生成解析器（包括Bit、Number类型）
        /// </summary>
        /// <param name="stylestr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static TermInterpreter Create(string stylestr, TermValue length)
        {
            if (!string.IsNullOrEmpty(stylestr))
            {
                InterpreterStyle style = (InterpreterStyle)Enum.Parse(typeof(InterpreterStyle), stylestr);
                switch (style)
                {
                    case InterpreterStyle.bit:
                        return new TermBitInterpreter(length);
                    case InterpreterStyle.number:
                        return new TermNumInterpreter(length);
                    case InterpreterStyle.bytes:
                        return new TermBytesInterpreter(length);
                    case InterpreterStyle.signed:
                        return new TermSignedInterpreter(length);
					case InterpreterStyle.none:
						return new TermNoneInterpreter (  );
                    case InterpreterStyle.str:
                    default:
                        return new TermStrInterpreter(length);
                }
            }
            return new TermNoneInterpreter();
        }
    }
}
