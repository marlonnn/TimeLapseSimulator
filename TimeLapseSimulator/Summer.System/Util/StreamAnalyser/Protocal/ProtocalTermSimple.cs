using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Value;

namespace Summer.System.Util.StreamAnalyser.Protocal
{
	/// <summary>
	/// 简单协议项类，包含预设数值
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-7-14
	/// </remark>
	[Obsolete ( "请使用ProtocalTerm代替" )]
	public class ProtocalTermSimple : ProtocalTerm
	{
		/// <summary>
		/// 预设数值
		/// </summary>
		public new TermValue Value { get; protected set; }

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="xmlVisitor"></param>
		/// <param name="protocalTerm"></param>
		public ProtocalTermSimple ( XmlVisitor xmlVisitor, List<XmlVisitor> xmlAliasList, ProtocalTerm protocalTerm = null )
			: base ( xmlVisitor, xmlAliasList, protocalTerm )
		{
			//   Value = new TermValue(xmlVisitor.GetAttribute("value"));
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="pt"></param>
		public ProtocalTermSimple ( ProtocalTermSimple pt )
			: base ( pt )
		{
			//     Value = pt.Value;
		}

		/// <summary>
		/// 复制生成ProtocalTerm对象
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public override ProtocalTerm Clone ( )
		{
			return new ProtocalTermSimple ( this );
		}
	}
}
