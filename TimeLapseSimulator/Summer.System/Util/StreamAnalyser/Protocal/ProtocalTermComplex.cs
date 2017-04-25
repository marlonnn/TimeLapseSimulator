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
	/// 复杂协议项类，包含多个协议项（顺序或选择方式）
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-7-14
	/// </remark>
	public class ProtocalTermComplex : ProtocalTerm
	{
		/// <summary>
		/// 协议项列表
		/// </summary>
		public ProtocalTermList PTList { get; protected set; }

		/// <summary>
		/// 构造函数
		/// </summary>
		public ProtocalTermComplex ( XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm pt = null )
			: base ( xmlVisitor, xmlAliasList, pt )
		{
			do
			{
				XmlVisitor xmlChild = xmlVisitor.FirstChild ( "sequence" );
				if ( xmlChild != null )
				{
					PTList = new ProtocalTermSequence ( xmlChild, xmlAliasList, this );
					break;
				}
				xmlChild = xmlVisitor.FirstChild ( "choice" );
				if ( xmlChild != null )
				{
					PTList = new ProtocalTermChoice ( xmlChild, xmlAliasList, this );
					break;
				}
			}
			while ( false );
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="pt"></param>
		public ProtocalTermComplex ( ProtocalTermComplex pt )
			: base ( pt )
		{
			if ( pt.PTList.IsVariant ( ) )
			{
				PTList = pt.PTList.Clone ( );
			}
		}

		/// <summary>
		/// 整合函数，用以关联协议的索引，在协议类全部生成后调用
		/// </summary>
		/// <param name="pd"></param>
		public override void Intergation ( ProtocalDesigner pd )
		{
			base.Intergation ( pd );
			PTList.Intergation ( pd );
		}

		/// <summary>
		/// 协议解析函数
		/// </summary>
		/// <param name="message">二进制数据流</param>
		/// <param name="index">数据流当前位置</param>
		/// <param name="restbit">当前位置的未解析的Bit数</param>
		/// <param name="io">解析参数类，暂未实现</param>
		/// <returns>返回解析结果</returns>
		/// <exception>解析失败会抛出异常</exception>
		public override MessageTermSlot Interpret ( byte[ ] message, ref InterpreterPosition pos, InterpretOpion io )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			long count = (long) Occurs;
			bool end = false;
			if ( count != 0 )
			{
				while ( count != 0 )
				{
					TermValue value = new TermValue ( "" );
					InterpreterPosition pos2 = new InterpreterPosition ( pos );
					if ( !( Interpreter is TermNoneInterpreter ) )
					{
						if ( !Interpreter.Interpret ( message, ref pos, out value, io ) )
						{//如果该解析项，解析失败，则需要抛出异常
							var log = Summer.System.Util.StreamAnalyser.Interpreter.InterpretLog.GetInstance ( );
							log.FullName = FullName;
							log.Pos = pos2;
							return null;
							//	throw new InterpreterException ( log.Clone ( ) as Interpreter.InterpretLog );
						}
						//该值被算出来后，如果其它部分也引用了此值，
						//则给其它部分赋值
						SetRelateTerm ( value );
					} else
					{
						//如果是无类型解析器，则Value直接赋值
						value = Value;
					}
					//然后对其Sequence中各项解析
					//注意1：Choice也是一种Sequence，
					//注意2：pos2不会受到pos在先前解析被改变的影响。
					//注意3：传入value，在Sequence中会用到					
					List<MessageTermSlot> mtList2 = PTList.Interpret ( message, ref pos2, ref end, Interpreter, value, io );
					if ( mtList2.Count == 0 )
					{
						if ( !( Interpreter is TermNoneInterpreter ) )
						{
							mtList2.Add ( CreateMessageTerm ( this, value ) );
						}
					}
					MessageTermSlot mt = CreateMessageTerm ( this, value, mtList2 );
					if ( mt != null )
					{
						mtList.Add ( mt );
					}
					if ( Interpreter is TermNoneInterpreter )
					{
						pos = new InterpreterPosition ( pos2 );
						if ( end )
						{
							break;
						}
					}
					count--;
				}
			}
			if ( mtList.Count ( ) == 0 )
			{
				mtList.Add ( CreateMessageTerm ( this ) );
			}
			return CreateMessageTerm ( this, Value, mtList );
		}

		public override MessageTermSlot Transfer ( XmlVisitor xmlVisitor )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			long count = (long) Occurs;
			List<XmlVisitor> xmlTermList = xmlVisitor.FilterChildren ( ).ToList ( ).FindAll ( r => r.GetAttribute ( "name" ) == Name );
			if ( xmlTermList.Count ( ) == 0 )
			{
				xmlTermList.Add ( xmlVisitor );
			}
			if ( count != 0 )
			{
				if ( ( count == -1 ) || ( count == xmlTermList.Count ) )
				{
					TermValue value = new TermValue ( "" );
					foreach ( XmlVisitor xmlTerm in xmlTermList )
					{
						if ( !( Interpreter is TermNoneInterpreter ) )
						{
							value = new TermValue ( xmlTerm.GetAttribute ( "value" ) );
							SetRelateTerm ( value );
						} else
						{
							value = Value;
						}
						List<MessageTermSlot> mtList2 = PTList.Transfer ( xmlTerm, Interpreter, value );
						if ( mtList2.Count == 0 )
						{
							if ( !( Interpreter is TermNoneInterpreter ) )
							{
								mtList2.Add ( CreateMessageTerm ( this, value ) );
							} else
							{
								mtList2.Add ( CreateMessageTerm ( this ) );
							}
						}
						MessageTermSlot mt = CreateMessageTerm ( this, value, mtList2 );
						if ( mt != null )
						{
							mtList.Add ( mt );
						}
					}
				}
			} else
			{
				MessageTermSlot mt = CreateMessageTerm ( this );
				if ( mt.XmlElementName == xmlVisitor.Name )
				{
					mtList.Add ( mt );
				}
			}
			return CreateMessageTerm ( this, Value, mtList );
		}

		public override MessageTermSlot CreateBlank ( bool ingoreoccurs = false )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			if ( !ingoreoccurs && Occurs.IsVariant ( ) )
			{
				mtList.Add ( CreateMessageTerm ( this ) );
			} else
			{
				long count = 1;
				if ( !ingoreoccurs )
				{
					count = (long) Occurs;
				}
				if ( count < 0 )
				{
					mtList.Add ( CreateMessageTerm ( this ) );
				} else
				{
					while ( count != 0 )
					{
						TermValue value = new TermValue ( "" );
						if ( !( Interpreter is TermNoneInterpreter ) )
						{
							value = Interpreter.CreateBlank ( );
						} else
						{
							value = Value;
						}
						List<MessageTermSlot> mtList2 = PTList.CreateBlank ( Interpreter, value );
						if ( mtList2.Count == 0 )
						{
							if ( !( Interpreter is TermNoneInterpreter ) )
							{
								mtList2.Add ( CreateMessageTerm ( this, value ) );
							} else
							{
								mtList2.Add ( CreateMessageTerm ( this ) );
							}
						}

						MessageTermSlot mt = CreateMessageTerm ( this, value, mtList2 );
						if ( mt != null )
						{
							mtList.Add ( mt );
						}
						count--;
					}
				}
			}
			return CreateMessageTerm ( this, Value, mtList );
		}

		public override MessageTermSlot CreateTerm ( List<MessageTermSlot> valuelist, InterpretOpion io )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			if ( Occurs.IsVariant ( ) )
			{
				Occurs.SetValue ( valuelist.Count ( ) );
			}
			long count = (long) Occurs;
			if ( -1 == count )
			{
				count = valuelist.Count;
			}
			if ( count != 0 )
			{
				if ( count == valuelist.Count )
				{
					for ( int i = 0; i < count; i++ )
					{
						byte[ ] message = null;
						if ( !( Interpreter is TermNoneInterpreter ) )
						{
							if ( Interpreter.IsVariant ( ) )
							{
								SetInterpreterVariant ( Interpreter, valuelist[ i ].Value );
							}
							if ( Interpreter.LengthByBit > 0 )
							{
								message = new byte[ Interpreter.LengthByBit / 8 + 1 ];
								InterpreterPosition pos = new InterpreterPosition ( );
								Interpreter.Serialize ( message, ref pos, valuelist[ i ].Value, io );
							}
						} else
						{
							message = new byte[ 1 ];
						}
						List<MessageTermSlot> mtList2 = PTList.CreateTerm ( message, this, valuelist[ i ], io );
						if ( mtList2.Count == 0 )
						{
							if ( !( Interpreter is TermNoneInterpreter ) )
							{
								mtList2.Add ( CreateMessageTerm ( this, valuelist[ i ].Value ) );
							} else
							{
								mtList2.Add ( CreateMessageTerm ( this ) );
							}
						}
						MessageTermSlot mt = CreateMessageTerm ( this, valuelist[ i ].Value, mtList2 );
						if ( mt != null )
						{
							mtList.Add ( mt );
						}
					}
				}
			} else
			{
				if ( valuelist.Count ( ) == 0 )
				{
					mtList.Add ( CreateMessageTerm ( this ) );
				}
			}
			return CreateMessageTerm ( this, Value, mtList );
		}

		public override bool VerifyContent ( MessageTermSlot mt )
		{
			long count = (long) mt.PT.Occurs;
			if ( 1 == count )
			{
				if ( !PTList.Vertify ( mt ) )
				{
					return false;
				}
			} else if ( count > 1 )
			{
				return base.VerifyContent ( mt );
			}
			return true;
		}

		/// <summary>
		/// 是否为变量
		/// </summary>
		/// <returns></returns>
		internal override bool IsVariant ( )
		{
			return ( base.IsVariant ( ) | PTList.IsVariant ( ) );
		}

		/// <summary>
		/// 复制生成ProtocalTerm对象
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public override ProtocalTerm Clone ( )
		{
			return new ProtocalTermComplex ( this );
		}

		internal override bool AddRelateTerm ( string name, TermValueRef tvr )
		{
			if ( name == Name )
			{
				base.AddRelateTerm ( name, tvr );
				return true;
			}
			foreach ( ProtocalTerm pt in PTList.Value )
			{
				if ( name == pt.Name )
				{
					pt.AddRelateTerm ( name, tvr );
					return true;
				}
			}
			return false;
		}

		internal override IEnumerable<bool> CreateBits ( StreamCreator.MessageConfig config )
		{
			List<bool> bits = new List<bool> ( );
			if ( PTList is ProtocalTermSequence )
			{
				var sequence_term = PTList as ProtocalTermSequence;
				foreach ( var term in sequence_term.Value )
					bits.AddRange ( term.CreateBits ( config ) );
			}else if (PTList is ProtocalTermChoice)
			{
				var choice_term = PTList as ProtocalTermChoice;
				//todo:choice的情况
			}
			return bits;
		}
	}
}
