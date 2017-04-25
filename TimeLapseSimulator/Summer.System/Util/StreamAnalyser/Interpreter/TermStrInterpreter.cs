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
	/// string类型解析类
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-7-21
	/// </remark>
	class TermStrInterpreter : TermInterpreter
	{
		/// <summary>
		/// 返回以bit计算的有效位长度
		/// </summary>
		/// <returns></returns>
		public override long LengthByBit
		{
			get
			{
				return (long) Length * 8;
			}
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="length"></param>
		public TermStrInterpreter ( TermValue length )
			: base ( length )
		{
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="ti"></param>
		public TermStrInterpreter ( TermStrInterpreter ti )
			: base ( ti )
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
		public override bool Interpret ( byte[ ] message, ref InterpreterPosition pos, out TermValue value, InterpretOpion io )
		{
			char[ ] val = null;
			int length = (int) (long) Length;
			if ( length > 0 )
			{
				val = new char[ length ];
			} else
			{
				val = new char[ 1 ];
			}
			var log = InterpretLog.GetInstance ( );
			do
			{
				if ( pos.index >= message.Count ( ) )//数组溢出
				{
					log.Message = InterpretLog.OutOfRange;
					goto error;
				}
				if ( pos.restbit != 8 )//未按字节对齐
				{
					log.Message = InterpretLog.ByteAlignError;
					goto error;
				}

				int restlength = length;
				while ( restlength > 0 )
				{
					if ( pos.index >= message.Count ( ) )//数组溢出
					{
						log.Message = InterpretLog.OutOfRange;
						goto error;
					}
					val[ length - restlength ] = (char) message[ pos.index ];
					pos.StepForward ( );
					restlength--;
				}
			}
			while ( false );
			value = new TermValue ( new string ( val ) );
			return true;
error:
			value = new TermValue ( new string ( val ) );
			return false;
		}

		public override TermValue CreateBlank ( )
		{
			return new TermValue ( "" );
		}

		/// <summary>
		/// 复制生成TermBitInterpreter对象
		/// </summary>
		/// <param name="ti"></param>
		/// <returns></returns>
		public override TermInterpreter Clone ( )
		{
			return new TermStrInterpreter ( this );
		}

		public override void ToXml ( XmlVisitor xmlvisitor )
		{
			xmlvisitor.UpdateAttribute ( "style", TermInterpreter.InterpreterStyle.str.ToString ( ) );
			xmlvisitor.UpdateAttribute ( "length", Length );
		}
	}
}
