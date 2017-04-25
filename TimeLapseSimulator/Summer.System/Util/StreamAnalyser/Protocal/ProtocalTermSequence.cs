using System.Collections.Generic;
using System.Linq;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Message;
using Summer.System.Util.StreamAnalyser.Interpreter;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Protocal
{
	/// <summary>
	/// 顺序协议项组类
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-7-14
	/// </remark>
	public class ProtocalTermSequence : ProtocalTermList
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public ProtocalTermSequence ( XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm pt = null )
			: base ( xmlVisitor, xmlAliasList, pt )
		{
			if ( xmlVisitor.Name == "sequence" )
			{
				foreach ( XmlVisitor xmlChild in xmlVisitor.FilterChildren ( "data" ) )
				{
					Value.AddRange ( ProtocalTerm.Create ( xmlChild, xmlAliasList, pt ) );
				}
			}
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="ptlist"></param>
		public ProtocalTermSequence ( ProtocalTermSequence ptlist )
			: base ( ptlist )
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
		public override List<MessageTermSlot> Interpret ( byte[ ] message, ref InterpreterPosition pos, ref bool end, TermInterpreter ti, TermValue value, InterpretOpion io )
		{
			if ( !( ti is TermNoneInterpreter ) )
			{
				//暂时不需处理
			}
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			foreach ( ProtocalTerm pt in Value )
			{
				MessageTermSlot mt = pt.Interpret ( message, ref pos, io );
				if ( mt != null )
				{
					mtList.Add ( mt );
				} else//错误提示
				{
					//对于Sequence中,如果一旦出现不能解析项,则该Sequence中的后续项解析终止
					break;
					//如果Sequence中有不能解析的情况，则报错
				}
			}
			return mtList;
		}

		public override List<MessageTermSlot> Transfer ( XmlVisitor xmlVisitor, TermInterpreter ti, TermValue value )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			List<XmlVisitor> xmlTermList = new List<XmlVisitor> ( );
			xmlTermList.AddRange ( xmlVisitor.FilterChildren ( ) );
			if ( Value.Count == xmlTermList.Count )
			{
				for ( int i = 0; i < Value.Count; i++ )
				{
					MessageTermSlot mt = Value[ i ].Transfer ( xmlTermList[ i ] );
					if ( mt != null )
					{
						mtList.Add ( mt );
					} else//错误提示
					{
					}
				}
			} else
			{

			}
			return mtList;
		}

		public override List<MessageTermSlot> CreateBlank ( TermInterpreter ti, TermValue value )
		{
			if ( !( ti is TermNoneInterpreter ) )
			{
				//暂时不需处理
			}
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			foreach ( ProtocalTerm pt in Value )
			{
				MessageTermSlot mt = pt.CreateBlank ( );
				if ( mt != null )
				{
					mtList.Add ( mt );
				} else//错误提示
				{
				}
			}
			return mtList;
		}

		public override List<MessageTermSlot> CreateTerm ( byte[ ] message, ProtocalTermComplex ptc, MessageTermSlot value, InterpretOpion io )
		{
			InterpreterPosition pos = new InterpreterPosition ( );
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			if ( !( ptc.Interpreter is TermNoneInterpreter ) )
			{
				foreach ( ProtocalTerm pt in Value )
				{
					MessageTermSlot mt = pt.Interpret ( message, ref pos, io );
					if ( mt != null )
					{
						mtList.Add ( mt );
					} else//错误提示
					{
					}
				}
			} else
			{
				MessageTermSlot mt = null;
				if ( value is MessageTermComplex )
				{
					mt = ProtocalTerm.CreateMessageTerm ( ptc, value.Value, ( value as MessageTermComplex ).MessageTermList );
				}
				if ( mt != null )
				{
					mtList.Add ( mt );
				}
			}
			return mtList;
		}

		public override bool Vertify ( MessageTermSlot mt )
		{
			MessageTermComplex mtc = mt as MessageTermComplex;
			if ( null == mtc )
			{
				return false;
			}
			if ( Value.Count ( ) != mtc.MessageTermList.Count ( ) )
			{
				return false;
			}
			for ( int i = 0; i < Value.Count ( ); i++ )
			{
				if ( !Value[ i ].Verify ( mt ) )
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 复制生成ProtocalTermList及其子类型对象
		/// </summary>
		/// <param name="ptlist"></param>
		/// <returns></returns>
		public override ProtocalTermList Clone ( )
		{
			return new ProtocalTermSequence ( this );
		}
	}
}
