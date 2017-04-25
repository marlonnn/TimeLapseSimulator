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
    /// 创建日期：2013-7-14
    /// </remark>
    public class TermNumInterpreter : TermInterpreter
    {
        /// <summary>
        /// 返回以bit计算的有效位长度
        /// </summary>
        /// <returns></returns>
        public override long LengthByBit
        {
            get
            {
                return (long)Length * 8;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="length"></param>
        public TermNumInterpreter(TermValue length)
            : base(length)
        {
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="ti"></param>
        public TermNumInterpreter(TermNumInterpreter ti)
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
            long val = 0;
            int length = (int)(long)Length;
	        var log = InterpretLog.GetInstance();
            do
            {
                if (pos.index >= message.Count())//数组溢出
                {//如果当前解析进度已经超过了msg的最大长度，则发生错误
	                log.Message = InterpretLog.OutOfRange;
                    goto error;
                }
                if (length > 8)//数值溢出
                {//作为一个Num，最大byte不能超过8位
					//long即Int64的长度为8Byte
	                log.Message = InterpretLog.LongerThanBit64;
                    goto error;
                }
                if (pos.restbit != 8)//未按字节对齐
                {//作为一个Num，开始Bit需对齐byte的始端
	                log.Message = InterpretLog.ByteAlignError;
                    goto error;
                }

                int restlength = length;
                while (restlength > 0)
                {
                    if (pos.index >= message.Count())//数组溢出
					{//如果当前解析进度已经超过了msg的最大长度，则发生错误
						log.Message = InterpretLog.OutOfRange;
                        goto error;
                    }
                    val = ByteData(val, message, pos, io);
                    pos.StepForward();
                    restlength--;
                }
            }
            while (false);
            value = new TermValue(val);
            return true;
        error:

            value = new TermValue(val);
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
            long val = (long)value;
            int length = (int)(long)Length;
            do
            {
                if (pos.index >= message.Count())//数组溢出
                {
                    goto error;
                }
                if (pos.restbit != 8)//未按字节对齐
                {
                    goto error;
                }

                int restlength = length;
                while (restlength > 0)
                {
                    message[pos.index] = BitData(val, restlength - 1, io);
                    pos.StepForward();
                    restlength--;
                    if (pos.index >= message.Count())//数组溢出
                    {
                        goto error;
                    }
                }
            }
            while (false);
            return true;
        error:
            return false;
        }

        public override TermValue CreateBlank()
        {
            return new TermValue(0);
        }

        /// <summary>
        /// 复制生成TermBytesInterpreter对象
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public override TermInterpreter Clone()
        {
            return new TermNumInterpreter(this);
        }

        public override void ToXml(XmlVisitor xmlvisitor)
        {
            xmlvisitor.UpdateAttribute("style", TermInterpreter.InterpreterStyle.number.ToString());
            xmlvisitor.UpdateAttribute("length", Length);
        }
		/// <summary>
		/// 把Value前移8bit，空出来的1个Byte，使用msg[pos]填充
		/// </summary>
		/// <param name="value"></param>
		/// <param name="message"></param>
		/// <param name="pos"></param>
		/// <param name="io"></param>
		/// <returns></returns>
        private long ByteData(long value, byte[] message, InterpreterPosition pos, InterpretOpion io)
        {
            return ((value << 8) + message[pos.index]);
        }

        private byte BitData(long value, int length, InterpretOpion io)
        {
            return (byte)(value >> (length * 8));
        }
    }
}
