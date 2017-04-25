using System.Collections.Generic;
using System.Linq;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Message;
using Summer.System.Util.StreamAnalyser.Interpreter;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Protocal
{
	/// <summary>
	/// 协议项类
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-7-14
	/// </remark>
	public class ProtocalTerm
	{
		public Dictionary<string, string> FieldMapping = new Dictionary<string, string> ( );
		/// <summary>
		/// 协议项名称
		/// </summary>
		public string Name { get; protected set; }

		public string FullName { get; protected set; }
		/// <summary>
		/// 解析器对象
		/// </summary>
		public TermInterpreter Interpreter { get; protected set; }

		/// <summary>
		/// 协议项说明
		/// </summary>
		public string Explain { get; protected set; }

		/// <summary>
		/// 协议项含义
		/// </summary>
		public TermValue Meaning { get; protected set; }

		/// <summary>
		/// 预设数值
		/// </summary>
		public TermValue Value { get; protected set; }

		/// <summary>
		/// 协议项次数
		/// </summary>
		public TermValue Occurs { get; protected set; }

		/// <summary>
		/// 结束标志
		/// </summary>
		public bool End { get; protected set; }

		/// <summary>
		/// 父级协议项对象
		/// </summary>
		public ProtocalTerm parentPT { get; set; }

		/// <summary>
		/// 相关数据值对象
		/// </summary>
		protected List<TermValueRef> relatedTerm;

		/// <summary>
		/// 构造函数
		/// </summary>
		public ProtocalTerm ( XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm pt = null )
		{
			#region 首先继承 父ProtocalTerm的各属性

			parentPT = pt;
			if ( pt != null )
			{
				//FieldMapping不继承
				//FieldMapping = pt.FieldMapping;
				Name = pt.Name;
				Interpreter = pt.Interpreter;
				Explain = pt.Explain;
				Meaning = pt.Meaning;
			}

			#endregion

			#region 读取Xml

			//把所有Attribute都读取
			foreach ( var pair in xmlVisitor.GetAllAttributes ( ) )
			{
				FieldMapping[ pair.Key ] = pair.Value;
			}


			string name = xmlVisitor.GetAttribute ( "name" );
			if ( !string.IsNullOrEmpty ( name ) )
			{
				Name = name;
			}

			#region 计算当前ProtocolTerm的FullName

			if ( pt != null )
			{
				if ( pt.Name == Name )
				{
					FullName = pt.FullName;
				} else
				{
					FullName = pt.FullName + "." + Name;
				}
			} else
			{
				FullName = Name;
			}

			#endregion

			string style = xmlVisitor.GetAttribute ( "style" );
			if ( !string.IsNullOrEmpty ( style ) )
			{
				Interpreter = TermInterpreter.Create ( style, CreateTermValue ( xmlVisitor, "length", "0" ) );
			}
			if ( null == Interpreter )
			{
				Interpreter = new TermNoneInterpreter ( );
			}

			string explain = xmlVisitor.GetAttribute ( "explain" );
			if ( !string.IsNullOrEmpty ( explain ) )
			{
				Explain = explain;
			}

			string meaning = xmlVisitor.GetAttribute ( "meaning" );
			if ( !string.IsNullOrEmpty ( meaning ) )
			{
				Meaning = CreateTermValue ( xmlVisitor, "meaning", "" );
			}
			if ( null == Meaning )
			{
				Meaning = new TermValue ( "" );
			}
			Occurs = CreateTermValue ( xmlVisitor, "occurs", "1" );
			Value = new TermValue ( xmlVisitor.GetAttribute ( "value" ) );
			if ( null == Value )
			{
				Value = new TermValue ( "" );
			}
			End = false;
			bool end = false;
			if ( bool.TryParse ( xmlVisitor.GetAttribute ( "end" ), out end ) )
			{
				End = end;
			}

			#endregion

			relatedTerm = new List<TermValueRef> ( );
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="pt"></param>
		public ProtocalTerm ( ProtocalTerm pt )
		{
			FieldMapping = pt.FieldMapping;
			Name = pt.Name;
			FullName = pt.FullName;
			Interpreter = pt.Interpreter.IsVariant ( ) ? pt.Interpreter.Clone ( ) : pt.Interpreter;
			Explain = pt.Explain;
			Meaning = pt.Meaning.IsVariant ( ) ? pt.Meaning.Clone ( ) : pt.Meaning;
			Occurs = pt.Occurs.IsVariant ( ) ? pt.Occurs.Clone ( ) : pt.Occurs;
			Value = pt.Value;
			End = pt.End;
			parentPT = pt.parentPT;
			relatedTerm = new List<TermValueRef> ( );
		}

		/// <summary>
		/// 生成bit数据
		/// </summary>
		/// <remarks>
		/// 作者：戴唯艺
		/// 日期：2014-5-4
		/// </remarks>
		internal virtual IEnumerable<bool> CreateBits ( StreamCreator.MessageConfig config )
		{
			var bits = config.GetBits ( this );
			return bits;
		}


		/// <summary>
		/// 整合函数，用以关联协议的索引，在协议类全部生成后调用
		/// </summary>
		/// <param name="pd"></param>
		public virtual void Intergation ( ProtocalDesigner pd )
		{
			if ( Occurs.IsVariant ( ) )
			{
				Occurs.Intergation ( pd.ExpressionFuncList, parentPT as ProtocalTermComplex );
			}
			if ( Meaning.IsVariant ( ) )
			{
				Meaning.Intergation ( pd.ExpressionFuncList, parentPT as ProtocalTermComplex );
			}
			if ( Interpreter.IsVariant ( ) )
			{
				Interpreter.Length.Intergation ( pd.ExpressionFuncList, parentPT as ProtocalTermComplex );
			}
		}

		/// <summary>
		/// 协议解析函数
		/// </summary>
		/// <param name="message">二进制数据流</param>
		/// <param name="index">数据流当前位置</param>
		/// <param name="restbit">当前位置的未解析的Bit数</param>
		/// <param name="io">解析参数类，暂未实现</param>
		/// <returns>返回解析结果</returns>
		public virtual MessageTermSlot Interpret ( byte[ ] message, ref InterpreterPosition pos, InterpretOpion io )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			if ( !( Interpreter is TermNoneInterpreter ) )
			{//如果Interpreter不是无类型解析器，则
				long count = (long) Occurs;
				if ( count != 0 )
				{
					while ( count > 0 )
					{
						TermValue value = null;
						if ( !Interpreter.Interpret ( message, ref pos, out value, io ) )
						{
							var log = StreamAnalyser.Interpreter.InterpretLog.GetInstance ( );
							log.FullName = FullName;
							log.Pos = pos;
							return null;
						}
						SetRelateTerm ( value );
						mtList.Add ( CreateMessageTerm ( this, value ) );
						count--;
					}
				} else
				{
					mtList.Add ( CreateMessageTerm ( this ) );
				}
			}
			return CreateMessageTerm ( this, Value, mtList );
		}

		public virtual MessageTermSlot Transfer ( XmlVisitor xmlVisitor )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			if ( !( Interpreter is TermNoneInterpreter ) )
			{
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
						foreach ( XmlVisitor xmlTerm in xmlTermList )
						{
							TermValue value = new TermValue ( xmlTerm.GetAttribute ( "value" ) );
							SetRelateTerm ( value );
							mtList.Add ( CreateMessageTerm ( this, value ) );
						}
					} else
					{
					}
				} else
				{
					MessageTermSlot mt = CreateMessageTerm ( this );
					if ( mt.XmlElementName == xmlVisitor.Name )
					{
						mtList.Add ( mt );
					}
				}
			}
			return CreateMessageTerm ( this, Value, mtList );
		}

		public virtual MessageTermSlot CreateBlank ( bool ingoreoccurs = false )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			if ( !( Interpreter is TermNoneInterpreter ) )
			{
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
					while ( count > 0 )
					{
						MessageTermSlot mt = null;
						if ( Interpreter.IsVariant ( ) )
						{
							mt = CreateMessageTerm ( this );
						} else
						{
							if ( string.IsNullOrEmpty ( (string) Value ) )
							{
								mt = CreateMessageTerm ( this, Interpreter.CreateBlank ( ) );
							} else
							{
								mt = CreateMessageTerm ( this, Value );
							}
						}
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

		public MessageTermSlot CreateTerm ( MessageTermSlot value, InterpretOpion io )
		{
			List<MessageTermSlot> valuelist = new List<MessageTermSlot> ( );
			valuelist.Add ( value );
			return CreateTerm ( valuelist, io );
		}

		public virtual MessageTermSlot CreateTerm ( List<MessageTermSlot> valuelist, InterpretOpion io )
		{
			List<MessageTermSlot> mtList = new List<MessageTermSlot> ( );
			if ( !( Interpreter is TermNoneInterpreter ) )
			{
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
					if ( count == valuelist.Count ( ) )
					{
						for ( int i = 0; i < valuelist.Count; i++ )
						{
							if ( Interpreter.IsVariant ( ) )
							{
								SetInterpreterVariant ( Interpreter, valuelist[ i ].Value );
							}
							mtList.Add ( CreateMessageTerm ( this, valuelist[ i ].Value ) );
						}
					}
				} else
				{
					if ( valuelist.Count ( ) == 0 )
					{
						mtList.Add ( CreateMessageTerm ( this ) );
					}
				}
			}
			return CreateMessageTerm ( this, Value, mtList );
		}

		public virtual bool Verify ( MessageTermSlot mt, bool ingoreoccurs = false )
		{
			if ( Name != mt.Name )
			{
				return false;
			}
			if ( null == mt.PT )
			{
				return false;
			}
			if ( !Interpreter.IsMatch ( mt.PT.Interpreter ) )
			{
				return false;
			}
			long count = (long) mt.PT.Occurs;
			if ( !ingoreoccurs )
			{
				if ( !Occurs.IsVariant ( ) )
				{
					if ( (long) Occurs != count )
					{
						return false;
					}
				}
			}
			return VerifyContent ( mt );
		}

		public virtual bool VerifyContent ( MessageTermSlot mt )
		{
			long count = (long) mt.PT.Occurs;
			if ( 1 == count )
			{
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
		internal virtual bool IsVariant ( )
		{
			if ( Occurs.IsVariant ( ) )
			{
				return true;
			}
			if ( Meaning.IsVariant ( ) )
			{
				return true;
			}
			if ( Interpreter.IsVariant ( ) )
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 复制生成ProtocalTerm对象
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public virtual ProtocalTerm Clone ( )
		{
			return new ProtocalTerm ( this );
		}

		/// <summary>
		/// 增加对对象至相关数据项列表
		/// </summary>
		/// <param name="name"></param>
		/// <param name="tvr"></param>
		internal virtual bool AddRelateTerm ( string name, TermValueRef tvr )
		{
			if ( name == Name )
			{
				relatedTerm.Add ( tvr );
				return true;
			}
			return false;
		}

		/// <summary>
		/// 根据数据值对象生成协议项解析对象
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		internal static MessageTermSlot CreateMessageTerm ( ProtocalTerm pt, TermValue value = null )
		{
			ProtocalTerm ptclone = pt.IsVariant ( ) ? pt.Clone ( ) : pt;
			if ( null == value )
			{
				return new MessageTermSlot ( ptclone );
			}
			return new MessageTerm ( ptclone, value );
		}

		/// <summary>
		/// 根据数据值对象生成协议项解析对象
		/// </summary>
		/// <param name="value"></param>
		/// <param name="mtList"></param>
		/// <returns></returns>
		internal static MessageTermSlot CreateMessageTerm ( ProtocalTerm pt, TermValue value, List<MessageTermSlot> mtList )
		{
			ProtocalTerm ptclone = pt.IsVariant ( ) ? pt.Clone ( ) : pt;
			if ( mtList.Count == 0 )
			{
				return null;
			} else if ( mtList.Count == 1 )
			{//如果List只有1个，则把该Mt的Name用传入pt的Name更新一下
				//Note:一般地，Sequence中只有一个的情况，表示与其父类型是同一个解析
				if ( mtList.First ( ).Name == pt.Name )
				{
					return mtList.First ( );
				}
			}
			MessageTermComplex mts = new MessageTermComplex ( ptclone, value, mtList );
			foreach ( MessageTermSlot mt in mtList )
			{
				mt.SetParent ( mts );
			}
			return mts;
		}

		protected void SetInterpreterVariant ( TermInterpreter ti, TermValue tv )
		{
			if ( ( ti is TermSignedInterpreter )
				|| ( ti is TermNumInterpreter )
				|| ( ti is TermBitInterpreter ) )
			{
				ti.Length.SetValue ( 4 );
			} else if ( ti is TermBytesInterpreter )
			{
				ti.Length.SetValue ( tv.value.Length / 2 );
			} else if ( ti is TermStrInterpreter )
			{
				ti.Length.SetValue ( tv.value );
			}
		}

		/// <summary>
		/// 设置相关数据数值
		/// </summary>
		/// <param name="tv"></param>
		protected void SetRelateTerm ( TermValue tv )
		{
			foreach ( TermValueRef tvr in relatedTerm )
			{
				tvr.SetValue ( tv );
			}
		}

		/// <summary>
		/// 根据XML片段生成TermValue或其子类
		/// </summary>
		/// <param name="xmlVisitor"></param>
		/// <param name="name"></param>
		/// <param name="defaultvalue"></param>
		/// <returns></returns>
		protected static TermValue CreateTermValue ( XmlVisitor xmlVisitor, string name, string defaultvalue )
		{
			TermValue vauletv = null;
			string value = xmlVisitor.GetAttribute ( name );
			if ( string.IsNullOrEmpty ( value ) )
			{
				vauletv = new TermValue ( defaultvalue );
			} else
			{
				vauletv = new TermValue ( value );
			}
			return TermValue.Create ( xmlVisitor.FirstChild ( name ), vauletv );
		}

		/// <summary>
		/// 根据XML片段生成ProtocalTerm及其子类对象
		/// </summary>
		public static List<ProtocalTerm> Create ( XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm pt = null )
		{
			List<ProtocalTerm> ptlist = new List<ProtocalTerm> ( );
			do
			{
				if ( ( xmlVisitor.FirstChild ( "choice" ) != null ) || ( xmlVisitor.FirstChild ( "sequence" ) != null ) )
				{
					if ( xmlVisitor.Name == "frame" )
					{
						ptlist.Add ( new ProtocalTermFrame ( xmlVisitor, xmlAliasList, pt ) );
					} else
					{
						ptlist.Add ( new ProtocalTermComplex ( xmlVisitor, xmlAliasList, pt ) );
					}
					break;
				}
				if ( !string.IsNullOrEmpty ( xmlVisitor.GetAttribute ( "frameref" ) ) || ( xmlVisitor.FirstChild ( "frame" ) != null ) )
				{
					ptlist.Add ( new ProtocalTermFramer ( xmlVisitor, xmlAliasList, pt ) );
					break;
				}
				if ( !string.IsNullOrEmpty ( xmlVisitor.GetAttribute ( "aliasref" ) ) )
				{
					//如果有aliasref的情况
					XmlVisitor xmlAlias = xmlAliasList.Find ( r => r.GetAttribute ( "name" ) == xmlVisitor.GetAttribute ( "aliasref" ) );
					if ( xmlAlias != null )
					{
						foreach ( XmlVisitor xmlChild in xmlAlias.FilterChildren ( "data" ) )
						{
							ptlist.AddRange ( Create ( xmlChild, xmlAliasList, pt ) );
						}
					}
					break;
				}
				ptlist.Add ( new ProtocalTerm ( xmlVisitor, xmlAliasList, pt ) );
				break;
			}
			while ( false );
			return ptlist;
		}
	}
}
