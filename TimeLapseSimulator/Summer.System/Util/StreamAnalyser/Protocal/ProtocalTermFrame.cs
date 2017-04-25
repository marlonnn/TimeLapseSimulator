using System.Collections.Generic;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Protocal
{
	/// <summary>
	/// 协议帧类
	/// </summary>
	public class ProtocalTermFrame : ProtocalTermComplex
	{
		/// <summary>
		/// 预设数值
		/// </summary>
		public new TermValue Value { get; protected set; }

		public List<TermValueRef> ParamList;
		Dictionary<string, List<TermValueRef>> relatedTermList;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="xmlVisitor"></param>
		/// <param name="pt"></param>
		public ProtocalTermFrame ( XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm pt = null )
			: base ( xmlVisitor, xmlAliasList, pt )
		{
			Value = new TermValue ( xmlVisitor.GetAttribute ( "value" ) );
			ParamList = new List<TermValueRef> ( );
			relatedTermList = new Dictionary<string, List<TermValueRef>> ( );
			foreach ( XmlVisitor xmlChild in xmlVisitor.FilterChildren ( "param" ) )
			{
				string paramname = xmlChild.GetAttribute ( "name" );
				ParamList.Add ( new TermValueRef ( paramname ) );
				relatedTermList[ paramname ] = new List<TermValueRef> ( );
			}
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="pt"></param>
		public ProtocalTermFrame ( ProtocalTermFrame pt )
			: base ( pt )
		{
			ParamList = new List<TermValueRef> ( pt.ParamList );
			relatedTermList = new Dictionary<string, List<TermValueRef>> ( pt.relatedTermList );
		}

		/// <summary>
		/// 复制生成ProtocalTerm对象
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public override ProtocalTerm Clone ( )
		{
			return new ProtocalTermFrame ( this );
		}

		/// <summary>
		/// 增加对对象至相关数据项列表
		/// </summary>
		/// <param name="tvr"></param>
		internal override bool AddRelateTerm ( string name, TermValueRef tvr )
		{
			if ( base.AddRelateTerm ( name, tvr ) )
			{
				return true;
			}
			if ( ParamList.Find ( r => r.ValueRef == name ) != null )
			{
				relatedTermList[ name ].Add ( tvr );
			}
			return true;
		}

		/// <summary>
		/// 设置相关数据数值
		/// </summary>
		/// <param name="tv"></param>
		internal void SetRelateTerm ( List<TermValue> tvlist )
		{
			if ( tvlist.Count == ParamList.Count )
			{
				for ( int i = 0; i < ParamList.Count; i++ )
				{
					string paramname = ParamList[ i ].ValueRef;
					foreach ( TermValueRef tvr in relatedTermList[ paramname ] )
					{
						tvr.SetValue ( tvlist[ i ] );
					}
				}
			}
		}

		
	}
}
