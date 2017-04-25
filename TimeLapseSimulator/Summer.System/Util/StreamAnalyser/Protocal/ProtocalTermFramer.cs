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
	/// 协议帧类
	/// </summary>
	public class ProtocalTermFramer : ProtocalTerm
	{
		/// <summary>
		/// 协议帧名称
		/// </summary>
		private string frameRefer { get; set; }

		/// <summary>
		/// 协议帧参数列表
		/// </summary>
		public List<TermValue> ParamList { get; set; }

		/// <summary>
		/// 协议帧对象
		/// </summary>
		public ProtocalTermFrame FrameRefer { get; set; }

		/// <summary>
		/// 协议帧参照值
		/// </summary>
		public TermValue AccordingTerm { get; set; }

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="xmlVisitor"></param>
		/// <param name="protocalTerm"></param>
		public ProtocalTermFramer ( XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm protocalTerm = null )
			: base ( xmlVisitor, xmlAliasList, protocalTerm )
		{
			frameRefer = xmlVisitor.GetAttribute ( "frameref" );
			XmlVisitor xmlChild = xmlVisitor.FirstChild ( "frame" );
			if ( xmlChild != null )
			{
				List<ProtocalTerm> ptlist = ProtocalTerm.Create ( xmlChild, xmlAliasList, this );
				if ( ptlist.Count > 0 )
				{
					FrameRefer = ptlist.First ( ) as ProtocalTermFrame;
				}
			}
			AccordingTerm = CreateTermValue ( xmlVisitor, "according", "" );
			ParamList = new List<TermValue> ( );
			foreach ( XmlVisitor xmlParam in xmlVisitor.FilterChildren ( "param" ) )
			{
				ParamList.Add ( TermValue.Create ( xmlParam, null ) );
			}
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="pt"></param>
		public ProtocalTermFramer ( ProtocalTermFramer pt )
			: base ( pt )
		{
			frameRefer = pt.frameRefer;
			FrameRefer = pt.FrameRefer;
			AccordingTerm = ( ( pt.AccordingTerm != null ) && pt.AccordingTerm.IsVariant ( ) ) ? pt.AccordingTerm.Clone ( ) : pt.AccordingTerm;
			ParamList = new List<TermValue> ( );
			foreach ( TermValue tv in pt.ParamList )
			{
				if ( ( tv != null ) && tv.IsVariant ( ) )
				{
					ParamList.Add ( tv.Clone ( ) );
				} else
				{
					ParamList.Add ( tv );
				}
			}
		}

		/// <summary>
		/// 整合函数，用以关联协议帧索引，在协议类全部生成后调用
		/// </summary>
		/// <param name="pd"></param>
		public override void Intergation ( ProtocalDesigner pd )
		{
			base.Intergation ( pd );
			if ( !string.IsNullOrEmpty ( frameRefer ) )
			{
				FrameRefer = pd.ProtocalTermList.Find ( r => r.Name == frameRefer );
			} else
			{
				FrameRefer.Intergation ( pd );
			}
			if ( ( AccordingTerm != null ) && AccordingTerm.IsVariant ( ) )
			{
				AccordingTerm.Intergation ( pd.ExpressionFuncList, parentPT as ProtocalTermComplex );
			}
			foreach ( TermValue tv in ParamList )
			{
				if ( ( tv != null ) && tv.IsVariant ( ) )
				{
					tv.Intergation ( pd.ExpressionFuncList, parentPT as ProtocalTermComplex );
				}
			}
		}

		/// <summary>
		/// 协议解析函数，含协议的判断和解析
		/// </summary>
		/// <param name="message">二进制数据流</param>
		/// <param name="index">数据流当前位置</param>
		/// <param name="restbit">当前位置的未解析的Bit数</param>
		/// <param name="io">解析参数类，暂未实现</param>
		/// <returns>返回解析结果</returns>
		public override MessageTermSlot Interpret ( byte[ ] message, ref InterpreterPosition pos, InterpretOpion io )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );

			long count = (long) Occurs;
			if ( count != 0 )
			{
				while ( count > 0 )
				{
					if ( !( Interpreter is TermNoneInterpreter ) )
					{
						TermValue value = null;
						InterpreterPosition pos2 = new InterpreterPosition ( pos );
						if ( !Interpreter.Interpret ( message, ref pos2, out value, io ) )
						{
							var log = StreamAnalyser.Interpreter.InterpretLog.GetInstance ( );
							log.FullName = FullName;
							log.Pos = pos2;

							if ( pos2.index >= message.Count ( ) )//数组尾
							{
								break;
							}
							//	throw new InterpreterException ( log.Clone ( ) as StreamAnalyser.Interpreter.InterpretLog );
						}
						if ( !value.Equals ( Value ) )
						{
							break;
						}
					} else if ( AccordingTerm != null )
					{//如果Frame引用为无类型解析器，则查看其According数据
						if ( !Value.Equals ( AccordingTerm ) )
						{
							break;
						}
					}
					//如果为无类型解析器，且能找到According数据，则开始解析具体的Frame
					if ( FrameRefer != null )
					{
						FrameRefer.SetRelateTerm ( ParamList );
						MessageTermSlot mt = FrameRefer.Interpret ( message, ref pos, io );
						if ( mt != null )
						{
							mtList.Add ( mt );
						} else
						{
							var log = StreamAnalyser.Interpreter.InterpretLog.GetInstance ( );
							log.FullName = FullName;
							log.Pos = pos;
							log.Message = StreamAnalyser.Interpreter.InterpretLog.FrameErrorInterept;
							//throw new InterpreterException ( log.Clone ( ) as StreamAnalyser.Interpreter.InterpretLog );
						}
					} else
					{
						var log = StreamAnalyser.Interpreter.InterpretLog.GetInstance ( );
						log.FullName = FullName;
						log.Pos = pos;
						log.Message = StreamAnalyser.Interpreter.InterpretLog.FramerNull;
						//		throw new InterpreterException ( log.Clone ( ) as StreamAnalyser.Interpreter.InterpretLog );
					}
					count--;
				}
			} else
			{
				mtList.Add ( CreateMessageTerm ( this ) );
			}
			//如果list为空，则返回null
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
			if ( ( count == -1 ) || ( count == xmlTermList.Count ) )
			{
				foreach ( XmlVisitor xmlTerm in xmlTermList )
				{
					TermValue value = new TermValue ( xmlTerm.GetAttribute ( "value" ) );
					if ( Value.Equals ( value ) )
					{
						if ( FrameRefer != null )
						{
							FrameRefer.SetRelateTerm ( ParamList );
							MessageTermSlot mt = FrameRefer.Transfer ( xmlTerm.FirstChild ( ) );
							if ( mt != null )
							{
								mtList.Add ( mt );
							} else
							{
								throw new Exception ( );
							}
						} else
						{
							throw new Exception ( );
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

		public override MessageTermSlot CreateTerm ( List<MessageTermSlot> valuelist, InterpretOpion io )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );

			if ( Occurs.IsVariant ( ) )
			{
				Occurs.SetValue ( valuelist.Count );
			}
			long count = (long) Occurs;
			if ( -1 == count )
			{
				count = valuelist.Count;
			}
			if ( count != 0 )
			{
				if ( count == valuelist.Count ( ) )
				{
					for ( int i = 0; i < valuelist.Count; i++ )
					{
						if ( !valuelist[ i ].Value.Equals ( Value ) )
						{
							break;
						}
						if ( ( AccordingTerm != null ) && AccordingTerm.IsVariant ( ) )
						{
							AccordingTerm.SetValue ( Value );
						}
						if ( ParamList.Count ( ) > 0 )
						{
							if ( ( valuelist[ i ].PT is ProtocalTermFrame ) && ( ParamList.Count ( ) == ( valuelist[ i ].PT as ProtocalTermFrame ).ParamList.Count ( ) ) )
							{
								for ( int index = 0; index < ParamList.Count ( ); index++ )
								{
									ParamList[ index ].SetValue ( (long) ( valuelist[ i ].PT as ProtocalTermFrame ).ParamList[ i ] );
								}
							} else
							{
							}
						}
						mtList.Add ( valuelist[ i ] );
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
				if ( null == FrameRefer )
				{
					return false;
				}
				return FrameRefer.Verify ( mt, false );
			} else if ( count > 1 )
			{
				MessageTermComplex mtc = mt as MessageTermComplex;
				if ( ( null == mtc ) || ( mtc.MessageTermList.Count != count ) )
				{
					return false;
				}
				foreach ( MessageTermSlot mts in mtc.MessageTermList )
				{
					if ( !Verify ( mts, true ) )
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// 是否为变量
		/// </summary>
		/// <returns></returns>
		internal override bool IsVariant ( )
		{
			if ( base.IsVariant ( ) )
			{
				return true;
			}
			if ( ( AccordingTerm != null ) && AccordingTerm.IsVariant ( ) )
			{
				return true;
			}
			foreach ( TermValue tv in ParamList )
			{
				if ( ( tv != null ) && tv.IsVariant ( ) )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 复制生成ProtocalTermAlias对象
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public override ProtocalTerm Clone ( )
		{
			return new ProtocalTermFramer ( this );
		}
		internal override IEnumerable<bool> CreateBits ( StreamCreator.MessageConfig config )
		{
			return FrameRefer != null ? FrameRefer.CreateBits ( config ) : new bool[ 0 ];
		}
	}
}
