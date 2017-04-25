using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Protocal;
using Summer.System.Util.StreamAnalyser.Interpreter;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Message
{
	/// <summary>
	/// 协议项解析类(未确定类)
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-7-14
	/// </remark>
	public class MessageTermSlot
	{

		/// <summary>
		/// 协议类
		/// </summary>
		public ProtocalTerm PT { get; protected set; }

		/// <summary>
		/// 数据项值
		/// </summary>
		public TermValue Value { get; protected set; }

		public string Name
		{
			get
			{
				if ( PT != null )
				{
					return PT.Name;
				}
				return string.Empty;
			}
		}

		public virtual string XmlElementName
		{
			get
			{
				return "msgTermSlot";
			}
		}

		public virtual long LengthByBit
		{
			get
			{
				return 0;
			}
		}

		public MessageTermComplex ParentTerm { get; protected set; }
	
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="value"></param>
		public MessageTermSlot ( ProtocalTerm pt )
		{
			PT = pt;
			ParentTerm = null;
		}

		public virtual bool Serialize ( byte[ ] message, ref InterpreterPosition pos, InterpretOpion io )
		{
			return true;
		}

		public virtual XmlVisitor ToXml ( )
		{
			XmlVisitor xmlTerm = XmlVisitor.Create ( XmlElementName );
			if ( PT != null )
			{
				xmlTerm.UpdateAttribute ( "name", Name );
				PT.Interpreter.ToXml ( xmlTerm );
			}
			if ( Value != null )
			{
				xmlTerm.UpdateAttribute ( "value", (string) Value );
			}
			if ( PT != null )
			{
				string meaning = (string) PT.Meaning;
				if ( !string.IsNullOrEmpty ( meaning ) )
				{
					xmlTerm.UpdateAttribute ( "meaning", (string) PT.Meaning );
				}
				xmlTerm.UpdateAttribute ( "protocal", PT.ToString ( ) );
			}
			if ( ParentTerm != null )
			{
				xmlTerm.UpdateAttribute ( "parent", ParentTerm.Name );
			}
			return xmlTerm;
		}

		public virtual bool UpdateTerm ( MessageTermSlot mt, MessageStream ms, InterpretOpion io )
		{
			if ( mt.PT != null )
			{
				if ( mt.PT.Occurs.IsVariant ( ) )
				{
					TermValueRef tvr = mt.PT.Occurs.Variant ( );
					if ( tvr != null )
					{
						UpdateRelateTerm ( tvr.ValueRef, new TermValue ( tvr.value ), ms, io );
					}
				}
				if ( mt.PT.Interpreter.IsVariant ( ) )
				{
					TermValueRef tvr = mt.PT.Interpreter.Length.Variant ( );
					if ( tvr != null )
					{
						UpdateRelateTerm ( tvr.ValueRef, new TermValue ( tvr.value ), ms, io );
					}
				}
				if ( mt.PT is ProtocalTermFramer )
				{
					ProtocalTermFramer ptf = mt.PT as ProtocalTermFramer;
					if ( ( ptf.AccordingTerm != null ) && ptf.AccordingTerm.IsVariant ( ) )
					{
						TermValueRef tvr = ptf.AccordingTerm.Variant ( );
						if ( tvr != null )
						{
							UpdateRelateTerm ( tvr.ValueRef, new TermValue ( tvr.value ), ms, io );
						}
					}
					foreach ( TermValue tv in ptf.ParamList )
					{
						TermValueRef tvr = tv.Variant ( );
						if ( tvr != null )
						{
							UpdateRelateTerm ( tvr.ValueRef, new TermValue ( tvr.value ), ms, io );
						}
					}
				}
			}
			if ( ParentTerm != null )
			{
				mt.SetParent ( ParentTerm );
				ParentTerm.ModifyTerm ( mt, ms, io );
			}
			return true;
		}

		public virtual bool ModifyTerm ( MessageTermSlot mt, MessageStream ms, InterpretOpion io )
		{
			return true;
		}

		protected bool UpdateRelateTerm ( string name, TermValue value, MessageStream ms, InterpretOpion io )
		{
			MessageTermComplex parentMT = ParentTerm;
			while ( parentMT != null )
			{
				MessageTermSlot mts = parentMT.MessageTermList.Find ( r => r.Name == name );
				if ( mts != null )
				{
					ProtocalTerm mtcPT = parentMT.PT;
					MessageTermSlot relateterm = null;
					if ( ms.CreateTerm ( mts.PT.FullName, new MessageTerm ( null, value ), out relateterm ) )
					{
						mts.UpdateTerm ( relateterm, ms, io );
					}
					break;
				}
				if ( parentMT.PT is ProtocalTermFrame )
				{
					ProtocalTermFrame ptf = parentMT.PT as ProtocalTermFrame;
					TermValue tv = ptf.ParamList.Find ( r => r.ValueRef == name );
					if ( tv != null )
					{
						tv.SetValue ( (long) value );
					}
				}
				parentMT = parentMT.ParentTerm;
			}
			return true;
		}

		public virtual void SetParent ( MessageTermComplex mtc )
		{
			ParentTerm = mtc;
		}
	}
}
