using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Protocal;
using Summer.System.Util.StreamAnalyser.Value;
using Summer.System.IO;

namespace Summer.System.Util.StreamAnalyser.Interpreter
{
    /// <summary>
    /// Bit类型解析类
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-7-14
    /// </remark>
    public class TermBitInterpreter : TermInterpreter
    {
        /// <summary>
        /// 返回以bit计算的有效位长度
        /// </summary>
        /// <returns></returns>
        public override long LengthByBit
        {
            get
            {
                return (long)Length;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="length"></param>
        public TermBitInterpreter(TermValue length)
            : base(length)
        {
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="ti"></param>
        public TermBitInterpreter(TermBitInterpreter ti)
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
                {
	                log.Message = InterpretLog.OutOfRange;
                    goto error;
                }
                if (length > 64)//数值溢出
                {//最长处理64个bit=8byte

	                log.Message = InterpretLog.LongerThanBit64;
                    goto error;
                }
                if (pos.restbit >= length)
                {//如果在一个byte中能处理话：
                    val = BitData(val, message, pos, length, io);
                    pos.StepForwardByBit(length);
                }
                else
                {//如果该数值跨Byte，则
                    val = BitData(val, message, pos, pos.restbit, io);
                    int restlength = length - pos.restbit;
                    pos.StepForwardByBit(pos.restbit);
                    while (restlength > 0)
                    {
                        if (pos.index >= message.Count())//数组溢出
                        {
                            goto error;
                        }
                        if (pos.restbit >= restlength)
                        {
                            val = BitData(val, message, pos, restlength, io);
                            pos.StepForwardByBit(restlength);
                            restlength = 0;
                        }
                        else
                        {
                            val = BitData(val, message, pos, pos.restbit, io);
                            pos.StepForward();
                            restlength -= pos.restbit;
                        }
                    }
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
                if (pos.restbit >= length)
                {
                    message[pos.index] |= BitData(val, pos.restbit, 0, length, io);
                    pos.StepForwardByBit(length);
                }
                else
                {
                    int restlength = length - pos.restbit;
                    message[pos.index] |= BitData(val, pos.restbit, restlength, pos.restbit, io);
                    pos.StepForwardByBit(pos.restbit);
                    while (restlength > 0)
                    {
                        if (pos.restbit >= restlength)
                        {
                            message[pos.index] |= BitData(val, pos.restbit, 0, restlength, io);
                            pos.StepForwardByBit(restlength);
                            restlength = 0;
                        }
                        else
                        {
                            restlength -= pos.restbit;
                            message[pos.index] |= BitData(val, pos.restbit, restlength, pos.restbit, io);
                            pos.StepForward();
                            if (pos.index >= message.Count())//数组溢出
                            {
                                goto error;
                            }
                        }
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
        /// 复制生成TermBitInterpreter对象
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public override TermInterpreter Clone()
        {
            return new TermBitInterpreter(this);
        }

        public override void ToXml(XmlVisitor xmlvisitor)
        {
            xmlvisitor.UpdateAttribute("style", TermInterpreter.InterpreterStyle.bit.ToString());
            xmlvisitor.UpdateAttribute("length", Length);
        }


        /// <summary>
        /// 根据当前位置和未解析的Bit数解析该字节剩余bit
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="restbit"></param>
        /// <param name="length"></param>
        /// <param name="io"></param>
        /// <returns></returns>
        private long BitData(long value, byte[] message, InterpreterPosition pos, int length, InterpretOpion io)
        {
            return ((value << length) + 
				//通过Mask消除前面bit
				((message[pos.index] >> (pos.restbit - length)) & BitMask((int)length)));
        }

        private byte BitData(long value, int restbit,int restdata, int length, InterpretOpion io)
        {
            return (byte)(((value >> restdata) & BitMask((int)length)) << (restbit - length));
        }

        /// <summary>
        /// 根据bit个获取取值掩码
        /// </summary>
        /// <param name="bitcount"></param>
        /// <returns></returns>
        private int BitMask(int bitcount)
        {
            int[] mask = new int[] { 0, 1, 3, 7, 15, 31, 63, 127, 255 };
            if ((bitcount < 0) || (bitcount > 8))
            {
                return 0;
            }
            return mask[bitcount];
        }
    }
}
