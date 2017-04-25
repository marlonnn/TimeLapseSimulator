using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Summer.System.IO
{
	/// <summary>
	/// XML解析处理类，含对XML文件和XML字符串的解析和操作。
	/// 通过此类载入xml数据，得到root节点后，就可以通过操作XmlVisitor对象来操作所有的节点。
	/// 此类包含的操作有：
	/// （1）静态初始化方法：
	/// CreateFromFile——从文件创建；
	/// CreateFromString——从xml字符串创建；
	/// CreateFromXmlHelper——克隆方式创建；
	/// （2）Root节点操作：
	/// SetRoot——设置根节点；
	/// GetRoot——得到根节点；
	/// （3）Declaration操作：
	/// SetDeclaration——设置；
	/// GetDeclaration——获取；
	/// （4）Comments操作：
	/// GetAllComments——获取所有注释；
	/// InsertFirstComment——在最前面插入一条注释；
	/// AppendCommnet——在最后面追加一条注释；
	/// （5）保存操作：
	/// Save2File——将xml文档保存到文件中去；
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-4-8
	/// </remark>
	public class XmlFileHelper
	{
		/// <summary>
		/// 使用XDocument保存XML数据
		/// </summary>
		protected XDocument docXML = null;

		/// <summary>
		/// 参数为XDocument的构造函数
		/// </summary>
		/// <param name="xDoc">XDocument参数</param>
		protected XmlFileHelper ( XDocument xDoc )
		{
			docXML = xDoc;
		}


		#region 创建XML解析对象
		// 作者：戴唯艺
		// 时间：2013-11-20
		/// <summary>
		/// 初始化一个包含XDocument的空内容对象
		/// </summary>
		public static XmlFileHelper CreateEmptyObject ( )
		{
			var xml = new XDocument ( );
			XmlFileHelper xf = new XmlFileHelper ( xml );
			return xf;
		}

		/// <summary>
		/// 根据文件创建XML解析类对象
		/// 函数可能抛出以下异常及其他异常：
		/// 文件不存在：throw FileNotFoundException
		/// 格式错误：throw FormatException
		/// </summary>
		/// <param name="fileName">文件名含绝对路径</param>
		/// <returns>返回XmlParse对象</returns>
		public static XmlFileHelper CreateFromFile ( string fileName )
		{
			if ( string.IsNullOrEmpty ( fileName ) )
			{
				throw new NullReferenceException ( );
			} else if ( !File.Exists ( fileName ) )
			{
				throw new FileNotFoundException ( fileName );
			}
			XmlFileHelper xmlParse = new XmlFileHelper ( XDocument.Load ( fileName, LoadOptions.SetLineInfo ) );
			if ( null == xmlParse.docXML )
			{
				xmlParse = null;
				throw new FormatException ( );
			}
			return xmlParse;
		}

		/// <summary>
		/// 根据字符串创建XML解析类对象,字符串为空则创建内容为空的XML解析类对象
		/// 函数可能抛出以下异常及其他异常：
		/// 格式错误：throw FormatException
		/// </summary>
		/// <param name="xmldata">XML数据字符串</param>
		/// <returns>返回XmlParse对象</returns>
		public static XmlFileHelper CreateFromString ( string xmldata )
		{
			XmlFileHelper xmlParse = null;
			if ( string.IsNullOrEmpty ( xmldata ) )
			{
				xmlParse = new XmlFileHelper ( new XDocument ( new XDeclaration ( "1.0.0", "utf-8", "yes" ) ) );
			} else
			{
				using ( XmlReader xmlReader = XmlReader.Create ( new StringReader ( xmldata ) ) )
				{
					xmlReader.MoveToContent ( );
					xmlParse = new XmlFileHelper ( XDocument.Load ( xmlReader, LoadOptions.SetLineInfo ) );
				}
			}
			if ( null == xmlParse.docXML )
			{
				xmlParse = null;
				throw new FormatException ( );
			}
			return xmlParse;
		}

		/// <summary>
		/// 根据根据已知XML解析对象对象复制创建新对象
		/// 函数可能抛出以下异常及其他异常：
		/// 字符串为空：NullReferenceException
		/// </summary>
		/// <param name="xmldata">XML数据字符串</param>
		/// <returns>返回XmlParse对象</returns>
		public static XmlFileHelper CreateFromXmlHelper ( XmlFileHelper xmlHelper )
		{
			if ( null == xmlHelper )
			{
				throw new NullReferenceException ( );
			}
			XmlFileHelper xmlParse = new XmlFileHelper ( xmlHelper.docXML );
			if ( null == xmlParse.docXML )
			{
				xmlParse = null;
				throw new FormatException ( );
			}
			return xmlParse;
		}
		#endregion

		/// <summary>
		/// 设置XML的Declaration
		/// </summary>
		/// <param name="version">XML版本</param>
		/// <param name="encoding">XML编码</param>
		/// <param name="standalone">XML是否关联外部约束文件</param>
		public void SetDeclaration ( string version, string encoding, string standalone )
		{
			docXML.Declaration = new XDeclaration ( version, encoding, standalone );
		}

		/// <summary>
		/// 返回XML的Declaration
		/// </summary>
		/// <param name="version">返回XML版本</param>
		/// <param name="encoding">返回XML编码</param>
		/// <param name="standalone">返回XML是否关联外部约束文件</param>
		public void GetDeclaration ( out string version, out string encoding, out string standalone )
		{
			version = string.Empty;
			encoding = string.Empty;
			standalone = string.Empty;
			if ( docXML.Declaration != null )
			{
				XDeclaration xmlDec = docXML.Declaration;
				version = xmlDec.Version;
				encoding = xmlDec.Encoding;
				standalone = xmlDec.Standalone;
			}
		}

		#region 注释相关操作
		/// <summary>
		/// 以迭代方式依次返回XML文档所有注释
		/// </summary>
		/// <returns>返回注释字符串</returns>
		public IEnumerable GetAllComments ( )
		{
			XNode node = docXML.FirstNode;
			while ( node != null )
			{
				if ( node is XComment )
				{
					yield return ( node as XComment ).Value;
				}
				node = node.NextNode;
			}
		}

		/// <summary>
		/// 添加注释到XML文档尾部（不删除已有的注释）
		/// </summary>
		/// <param name="values">要添加的注释信息列表</param>
		public void AppendComment ( List<string> values )
		{
			XNode node = null;
			if ( 0 == values.Count )
			{
				return;
			}
			int index = 0;
			node = docXML.LastNode;
			if ( null == node )
			{
				node = new XComment ( values[ 0 ] );
				docXML.Add ( node );
				index++;
			}
			if ( node != null )
			{
				for ( ; index < values.Count; index++ )
				{
					node.AddAfterSelf ( new XComment ( new XComment ( values[ index ] ) ) );
				}
			}
		}

		/// <summary>
		/// 添加注释到XML文档首部（不删除已有的注释）
		/// </summary>
		/// <param name="values">要添加的注释信息列表</param>
		public void InsertFirstComment ( List<string> values )
		{
			XNode node = null;
			int count = values.Count;
			if ( 0 == count )
			{
				return;
			}
			node = docXML.FirstNode;
			if ( null == node )
			{
				node = new XComment ( values[ count - 1 ] );
				docXML.Add ( node );
				count--;
			}
			if ( node != null )
			{
				for ( int index = 0; index < count; index++ )
				{
					node.AddBeforeSelf ( new XComment ( new XComment ( values[ index ] ) ) );
				}
			}
		}

		/// <summary>
		/// 删除XML文档所有注释,不删除子节点的注释
		/// </summary>
		public void RemoveAllComment ( )
		{
			XNode node = docXML.FirstNode;
			while ( node != null )
			{
				XNode nextnode = node.NextNode;
				if ( node is XComment )
				{
					node.Remove ( );
				}
				node = nextnode;
			}
		}
		#endregion

		#region 元素相关操作
		/// <summary>
		/// 返回根元素
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor GetRoot ( )
		{
			return XmlVisitor.Create ( docXML.Root );
		}

		/// <summary>
		/// 设置根元素
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor SetRoot ( string name, object value = null )
		{
			XElement xmlEle = XmlOperator.CreateXmlElement ( name, value );
			if ( docXML.Root != null )
			{
				docXML.RemoveNodes ( );
			}
			docXML.Add ( xmlEle );
			return XmlVisitor.Create ( xmlEle );
		}

		/// <summary>
		/// 设置根元素
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		public void SetRoot ( XmlVisitor xmlVisitor )
		{
			if ( xmlVisitor != null )
			{
				if ( docXML.Root != null )
				{
					docXML.RemoveNodes ( );
				}
				docXML.Add ( xmlVisitor.eleXML );
			}
		}

		/// <summary>
		/// 删除根元素(此操作将删除所有节点)
		/// </summary>
		public void RemoveRoot ( )
		{
			docXML.RemoveNodes ( );
		}
		#endregion

		/// <summary>
		/// 保存XML至文件
		/// 函数可能抛出以下异常及其他异常：
		/// 文件名为空：throw FileNotFoundException
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>返回操作结果</returns>
		public bool Save2File ( string fileName )
		{
			if ( string.IsNullOrEmpty ( fileName ) )
			{
				throw new NullReferenceException ( );
			}
			docXML.Save ( fileName );
			return true;
		}
	}

	/// <summary>
	/// XML元素访问类，一个XmlVisitor实例代表Xml文档的一个节点，对此节点的操作共有以下几类：
	/// （1）新增操作：
	/// Insert——在当前节点前面插入一个节点；
	/// Append——在当前节点后面追加一个节点；
	/// InsertFirstChild——在当前节点中插入一个孩子节点到第一个位置；
	/// AppendChild——在当前节点中追加一个孩子节点到最后一个位置；
	/// （2）修改操作：
	/// UpdateAttribute——针对当前节点的属性修改操作；
	/// （3）删除操作：
	/// Remove——删除当前节点；
	/// RemoveChildren——删除当前节点的所有孩子节点；
	/// （4）路径操作：
	/// Parent——当前节点的父节点；
	/// Prev——当前节点的前一个兄弟节点；
	/// Next——当前节点的后一个兄弟节点；
	/// Children——当前节点的子节点（不包括孙子等下级节点）
	/// （5）路径操作的增强操作：
	/// FirstChild——当前节点的第一个孩子节点；
	/// LastChild——当前节点的最后一个孩子节点；
	/// （6）属性操作
	/// GetAttribute——获得当前节点的某个属性值
	/// GetAllAttributes——获得当前节点的所有属性键值对
	/// RemoveAttribute——删除当前节点的指定属性；
	/// UpdateAttribute——更新当前节点的指定属性值；
	/// （7）注释类操作
	/// （8）运算符重载
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-4-8
	/// </remark>
	public class XmlVisitor
	{
		/// <summary>
		/// 使用XDocument保存XML数据
		/// </summary>
		internal XElement eleXML = null;

		/// <summary>
		/// 参数为XElement的构造函数
		/// </summary>
		/// <param name="xEle">XElement参数</param>
		protected XmlVisitor ( XElement xEle )
		{
			eleXML = xEle;
		}

		/// <summary>
		/// 根据XElement创建XML访问类对象
		/// </summary>
		/// <param name="xEle"></param>
		/// <returns>XML访问类对象</returns>
		static internal XmlVisitor Create ( XElement xEle )
		{
			if ( null == xEle )
			{
				return null;
			}
			return new XmlVisitor ( xEle );
		}

		/// <summary>
		/// 根据名称和值创建XML访问类对象
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>XML访问类对象</returns>
		static public XmlVisitor Create ( string name, object value = null )
		{
			return Create ( XmlOperator.CreateXmlElement ( name, value ) );
		}

		/// <summary>
		/// 根据XmlVisitor创建XML访问类对象
		/// </summary>
		/// <param name="xVisitor"></param>
		/// <returns>XML访问类对象</returns>
		static public XmlVisitor Create ( XmlVisitor xVisitor )
		{
			if ( null == xVisitor )
			{
				return null;
			}
			return Create ( new XElement ( xVisitor.eleXML ) );
		}

		#region 注释相关操作
		/// <summary>
		/// 以迭代方式依次返回所有注释
		/// </summary>
		/// <returns>返回注释字符串</returns>
		public IEnumerable Comments ( )
		{
			XNode node = node = eleXML.FirstNode;
			while ( node != null )
			{
				if ( node is XComment )
				{
					yield return ( node as XComment ).Value;
				}
				node = node.NextNode;
			}
		}

		/// <summary>
		/// 添加注释到尾部(不删除已有的注释)
		/// </summary>
		/// <param name="values">要添加的注释信息列表</param>
		public void AppendComment ( List<string> values )
		{
			XNode node = null;
			if ( 0 == values.Count )
			{
				return;
			}
			int index = 0;
			node = eleXML.LastNode;
			if ( null == node )
			{
				node = new XComment ( values[ 0 ] );
				eleXML.Add ( node );
				index++;
			}
			if ( node != null )
			{
				for ( ; index < values.Count; index++ )
				{
					node.AddAfterSelf ( new XComment ( new XComment ( values[ index ] ) ) );
				}
			}
		}

		/// <summary>
		/// 添加注释到首部(不删除已有的注释)
		/// </summary>
		/// <param name="values">要添加的注释信息列表</param>
		public void InsertFirstComment ( List<string> values )
		{
			XNode node = null;
			int count = values.Count;
			if ( 0 == count )
			{
				return;
			}
			node = eleXML.FirstNode;
			if ( null == node )
			{
				node = new XComment ( values[ count - 1 ] );
				eleXML.Add ( node );
				count--;
			}
			if ( node != null )
			{
				for ( int index = 0; index < count; index++ )
				{
					node.AddBeforeSelf ( new XComment ( new XComment ( values[ index ] ) ) );
				}
			}
		}

		/// <summary>
		/// 删除所有注释(不删除子节点的注释)
		/// </summary>
		public void RemoveAllComment ( )
		{
			XNode node = eleXML.FirstNode;
			while ( node != null )
			{
				XNode nextnode = node.NextNode;
				if ( node is XComment )
				{
					node.Remove ( );
				}
				node = nextnode;
			}
		}
		#endregion

		#region 元素相关操作

		/// <summary>
		/// 返回当前节点的父节点，如果是根节点，则返回null；
		/// </summary>
		/// <returns></returns>
		public XmlVisitor Parent ( )
		{
			return XmlVisitor.Create ( eleXML.Parent );
		}

		/// <summary>
		/// 返回当前节点的上一个指定名称的元素（同级节点）
		/// name为空不考虑名称限制
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor Prev ( string name = "" )
		{
			IEnumerable<XElement> xmlEleList = null;
			if ( string.IsNullOrEmpty ( name ) )
			{
				xmlEleList = eleXML.ElementsBeforeSelf ( );
			} else
			{
				xmlEleList = eleXML.ElementsBeforeSelf ( name );
			}
			if ( xmlEleList.Count ( ) > 0 )
			{
				return XmlVisitor.Create ( xmlEleList.Last ( ) );

			}
			return null;
		}

		/// <summary>
		/// 返回当前节点的下一个指定名称的元素（同级节点）
		/// name为空不考虑名称限制
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor Next ( string name = "" )
		{
			IEnumerable<XElement> xmlEleList = null;
			if ( string.IsNullOrEmpty ( name ) )
			{
				xmlEleList = eleXML.ElementsAfterSelf ( );
			} else
			{
				xmlEleList = eleXML.ElementsAfterSelf ( name );
			}
			if ( xmlEleList.Count ( ) > 0 )
			{
				return XmlVisitor.Create ( xmlEleList.First ( ) );

			}
			return null;
		}

		/// <summary>
		/// 获得当前节点下的所有子节点（不包括孙子及以下级别节点），如果为空返回null
		/// </summary>
		/// <returns></returns>
		public IEnumerable<XmlVisitor> Children ( )
		{
			return FilterChildren ( "" );
		}

		/// <summary>
		/// 过滤当前节点下指定名称的所有子节点（不包括孙子及以下级别节点），如果为空返回null。
		/// 如果name为空不考虑名称限制，等同于Children函数；
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public IEnumerable<XmlVisitor> FilterChildren ( string name = "" )
		{
			IEnumerable<XElement> xmlEleList = null;
			if ( string.IsNullOrEmpty ( name ) )
			{
				xmlEleList = eleXML.Elements ( );
			} else
			{
				xmlEleList = eleXML.Elements ( name );
			}
			if ( xmlEleList != null )
			{
				foreach ( var xmlEle in xmlEleList )
				{
					yield return XmlVisitor.Create ( xmlEle );
				}
			}
		}

		/// <summary>
		/// 返回当前节点的子节点集合中的第一个元素
		/// name为空不考虑名称限制
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor FirstChild ( string name = "" )
		{
			IEnumerable<XElement> xmlEleList = null;
			if ( string.IsNullOrEmpty ( name ) )
			{
				xmlEleList = eleXML.Elements ( );
			} else
			{
				xmlEleList = eleXML.Elements ( name );
			}
			if ( xmlEleList.Count ( ) > 0 )
			{
				return XmlVisitor.Create ( xmlEleList.First ( ) );

			}
			return null;
		}

		/// <summary>
		/// 返回当前节点的子节点集合中的最后一个元素
		/// name为空不考虑名称限制
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor LastChild ( string name = "" )
		{
			IEnumerable<XElement> xmlEleList = null;
			if ( string.IsNullOrEmpty ( name ) )
			{
				xmlEleList = eleXML.Elements ( );
			} else
			{
				xmlEleList = eleXML.Elements ( name );
			}
			if ( xmlEleList.Count ( ) > 0 )
			{
				return XmlVisitor.Create ( xmlEleList.Last ( ) );

			}
			return null;
		}

		/// <summary>
		/// 以迭代方式依次返回所有父元素（不含自身）
		/// name为空不考虑名称限制
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public IEnumerable<XmlVisitor> Ancestors ( string name = "" )
		{
			IEnumerable<XElement> xmlEleList = null;
			if ( string.IsNullOrEmpty ( name ) )
			{
				xmlEleList = eleXML.Ancestors ( );
			} else
			{
				xmlEleList = eleXML.Ancestors ( name );
			}
			foreach ( var xmlEle in xmlEleList )
			{
				yield return XmlVisitor.Create ( xmlEle );
			}
		}

		/// <summary>
		/// 以迭代方式依次返回所有父元素（含自身）
		/// name为空不考虑名称限制
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <returns>返回XMLVisitor对象</returns>
		public IEnumerable<XmlVisitor> AncestorsAndSelf ( string name = "" )
		{
			IEnumerable<XElement> xmlEleList = null;
			if ( string.IsNullOrEmpty ( name ) )
			{
				xmlEleList = eleXML.AncestorsAndSelf ( );
			} else
			{
				xmlEleList = eleXML.AncestorsAndSelf ( name );
			}
			foreach ( var xmlEle in xmlEleList )
			{
				yield return XmlVisitor.Create ( xmlEle );
			}
		}

		/// <summary>
		/// 根据XPath返回首个元素,XPath格式为:名称1.子名称2.子名称3
		/// 当查找不到时返回为空
		/// </summary>
		/// <param name="path">元素相对路径，以.号间隔路径</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor FirstChildByPath ( string path )
		{
			string p = path.Replace ( ".", "/" );
			return XmlVisitor.Create ( eleXML.XPathSelectElement ( p ) );
		}

		/// <summary>
		/// 根据XPath返回元素列表,XPath格式为:名称1.子名称2.子名称3
		/// 当查找不到时返回为空
		/// </summary>
		/// <param name="path">元素相对路径</param>
		/// <returns>返回XMLVisitor对象</returns>
		public IEnumerable<XmlVisitor> ChildrenByPath ( string path )
		{
			string p = path.Replace ( ".", "/" );
			foreach ( var xmlEle in eleXML.XPathSelectElements ( p ) )
			{
				yield return XmlVisitor.Create ( xmlEle );
			}
		}

		/// <summary>
		/// 在当前节点下添加子元素至尾部
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor AppendChild ( string name, object value = null )
		{
			XElement xmlEle = XmlOperator.CreateXmlElement ( name, value );
			if ( xmlEle != null )
			{
				eleXML.Add ( xmlEle );
				return XmlVisitor.Create ( xmlEle );
			}
			return null;
		}

		/// <summary>
		/// 在当前节点下添加子元素至首部
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor InsertFirstChild ( string name, object value = null )
		{
			XElement xmlEle = XmlOperator.CreateXmlElement ( name, value );
			if ( xmlEle != null )
			{
				eleXML.AddFirst ( xmlEle );
				return XmlVisitor.Create ( xmlEle );
			}
			return null;
		}

		/// <summary>
		/// 在当前节点之后添加元素
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor Append ( string name, object value = null )
		{
			XElement xmlEle = XmlOperator.CreateXmlElement ( name, value );
			if ( xmlEle != null )
			{
				eleXML.AddAfterSelf ( xmlEle );
				return XmlVisitor.Create ( xmlEle );
			}
			return null;
		}

		/// <summary>
		/// 在当前节点之前位置添加元素
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>返回XMLVisitor对象</returns>
		public XmlVisitor Insert ( string name, object value = null )
		{
			XElement xmlEle = XmlOperator.CreateXmlElement ( name, value );
			if ( xmlEle != null )
			{
				eleXML.AddBeforeSelf ( xmlEle );
				return XmlVisitor.Create ( xmlEle );
			}
			return null;
		}

		/// <summary>
		/// 在当前节点下添加XmlVisitor子元素对象至尾部
		/// </summary>
		/// <param name="xmlVisitor">XmlVisitor对象</param>
		public void AppendChild ( XmlVisitor xmlVisitor )
		{
			if ( xmlVisitor != null )
			{
				eleXML.Add ( xmlVisitor.eleXML );
			}
		}

		/// <summary>
		/// 在当前节点下添加XmlVisitor子元素至首部
		/// </summary>
		/// <param name="xmlVisitor">XmlVisitor对象</param>
		public void InsertFirstChild ( XmlVisitor xmlVisitor )
		{
			if ( xmlVisitor != null )
			{
				eleXML.AddFirst ( xmlVisitor.eleXML );
			}
		}

		/// <summary>
		/// 在当前节点之后位置添加元素（同级元素添加）
		/// </summary>
		/// <param name="xmlVisitor">XmlVisitor对象</param>
		public void Insert ( XmlVisitor xmlVisitor )
		{
			if ( xmlVisitor != null )
			{
				eleXML.AddAfterSelf ( xmlVisitor.eleXML );
			}
		}

		/// <summary>
		/// 在当前节点之前位置添加元素（同级元素添加）
		/// </summary>
		/// <param name="xmlVisitor">XmlVisitor对象</param>
		public void Append ( XmlVisitor xmlVisitor )
		{
			if ( xmlVisitor != null )
			{
				eleXML.AddBeforeSelf ( xmlVisitor.eleXML );
			}
		}

		/// <summary>
		/// 删除当前元素
		/// </summary>
		public void Remove ( )
		{
			eleXML.Remove ( );
		}

		/// <summary>
		/// 删除所有子元素,不删除属性
		/// </summary>
		public void RemoveChildren ( )
		{
			eleXML.RemoveNodes ( );
		}
		#endregion

		#region 属性相关操作

		/// <summary>
		/// 获得当前节点的某个属性值
		/// </summary>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		public string GetAttribute ( string attributeName )
		{
			var attrlist = GetAllAttributes ( );
			string attrvalue = string.Empty;
			if ( attrlist.ContainsKey ( attributeName ) )
			{
				attrvalue = GetAllAttributes ( )[ attributeName ];
			}
			return attrvalue;
		}
		/// <summary>
		/// [修订: 戴唯艺(2014-02-20)]
		/// 功能: 把属性值当成10进制, 取Int32类型的数据, 如果读取失败, 返回默认值
		/// </summary>
		/// <param name="attributeName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public Int32 GetInt32Attribute ( string attributeName, int defaultValue = 0 )
		{
			var str = GetAttribute ( attributeName );
			Int32 rst;
			if ( !Int32.TryParse ( str, out rst ) )
				rst = defaultValue;
			return rst;
		}

		/// <summary>
		/// 返回所有属性值
		/// </summary>
		/// <returns>返回<Name, Value>属性列表</returns>
		public Dictionary<string, string> GetAllAttributes ( )
		{
			Dictionary<string, string> attrList = new Dictionary<string, string> ( );
			foreach ( var xmlAttr in eleXML.Attributes ( ) )
			{
				attrList[ xmlAttr.Name.ToString ( ) ] = xmlAttr.Value;
			}
			return attrList;
		}

		/// <summary>
		/// 设置当前节点的某个属性值
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <param name="value">属性值</param>
		public void UpdateAttribute ( string name, object value )
		{
			XAttribute xmlAttr = eleXML.Attribute ( name );
			if ( xmlAttr != null )
			{
				xmlAttr.SetValue ( value );
			} else
			{
				eleXML.Add ( new XAttribute ( name, value ) );
			}
		}

		/// <summary>
		/// 设置当前节点的某个属性值(在指定命名空间下）
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <param name="value">属性值</param>
		public void UpdateAttribute ( string spacename, string name, object value )
		{
			if ( string.Empty == spacename )
			{
				UpdateAttribute ( name, value );
				return;
			}
			XNamespace xmlNamespace = XmlOperator.CreateXNamespace ( spacename );

			XAttribute xmlAttr = eleXML.Attribute ( xmlNamespace + name );
			if ( xmlAttr != null )
			{
				xmlAttr.SetValue ( value );
			} else
			{
				eleXML.Add ( new XAttribute ( xmlNamespace + name, value ) );
			}
		}

		/// <summary>
		/// 删除当前节点的某个属性值，当name为空时删除所有属性
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <param name="value">属性值</param>
		public void RemoveAttribute ( string name )
		{
			if ( !string.IsNullOrEmpty ( name ) )
			{
				XAttribute xmlAttr = eleXML.Attribute ( name );
				if ( xmlAttr != null )
				{
					xmlAttr.Remove ( );
				}
			} else
			{
				eleXML.RemoveAttributes ( );
			}
		}

		/// <summary>
		/// 删除当前节点的某个属性值，当name为空时删除所有属性(在Xmlns命名空间下）
		/// </summary>
		/// <param name="spacename">命名空间</param>
		/// <param name="name">属性名称</param>
		/// <param name="value">属性值</param>
		public void RemoveAttribute ( string spacename, string name )
		{
			if ( string.Empty == spacename )
			{
				RemoveAttribute ( name );
				return;
			}
			XNamespace xmlNamespace = ( spacename );
			if ( !string.IsNullOrEmpty ( name ) )
			{
				XAttribute xmlAttr = eleXML.Attribute ( xmlNamespace + name );
				if ( xmlAttr != null )
				{
					xmlAttr.Remove ( );
				}
			} else
			{
				eleXML.RemoveAttributes ( );
			}
		}
		#endregion


		/// <summary>
		/// 元素名称
		/// </summary>
		public string Name
		{
			get
			{
				return eleXML.Name.ToString ( );
			}
			set
			{
				eleXML.Name = value;
			}
		}

		/// <summary>
		/// 元素值
		/// </summary>
		public string Value
		{
			get
			{
				return eleXML.Value;
			}
			set
			{
				eleXML.Value = value;
			}
		}

		/// <summary>
		/// 是否有子元素
		/// </summary>
		public bool HasChildren
		{
			get
			{
				return eleXML.Elements ( ).Count ( ) > 0;
			}
		}

		/// <summary>
		/// 强制转换为string，未处理底层异常
		/// </summary>
		/// <param name="xVisitor">XML访问类</param>
		/// <returns></returns>
		static public explicit operator string ( XmlVisitor xVisitor )
		{
			return (string) xVisitor.eleXML;
		}

		/// <summary>
		/// 强制转换为int，未处理底层异常
		/// </summary>
		/// <param name="xVisitor">XML访问类</param>
		/// <returns></returns>
		static public explicit operator int ( XmlVisitor xVisitor )
		{
			return (int) xVisitor.eleXML;
		}

		/// <summary>
		/// 强制转换为bool，未处理底层异常
		/// </summary>
		/// <param name="xVisitor">XML访问类</param>
		/// <returns></returns>
		static public explicit operator bool ( XmlVisitor xVisitor )
		{
			return (bool) xVisitor.eleXML;
		}

		/// <summary>
		/// 强制转换为double，未处理底层异常
		/// </summary>
		/// <param name="xVisitor">XML访问类</param>
		/// <returns></returns>
		static public explicit operator double ( XmlVisitor xVisitor )
		{
			return (double) xVisitor.eleXML;
		}

		/// <summary>
		/// 强制转换为DateTime，未处理底层异常
		/// </summary>
		/// <param name="xVisitor">XML访问类</param>
		/// <returns></returns>
		static public explicit operator DateTime ( XmlVisitor xVisitor )
		{
			return (DateTime) xVisitor.eleXML;
		}
	}

	/// <summary>
	/// XML操作类
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-4-15
	/// </remark>
	internal static class XmlOperator
	{
		/// <summary>
		/// 根据name和value创建XElemet
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>返回XElement对象</returns>
		public static XElement CreateXmlElement ( string name, object value )
		{
			if ( !string.IsNullOrEmpty ( name ) )
			{
				if ( null == value )
				{
					return new XElement ( name );
				} else
				{
					return new XElement ( name, value );
				}
			}
			return null;
		}

		/// <summary>
		/// 根据name和value创建XElemet
		/// </summary>
		/// <param name="name">元素名称</param>
		/// <param name="value">元素值</param>
		/// <returns>返回XElement对象</returns>
		public static XElement CreateXmlElement ( string spacename, string name, object value )
		{
			if ( string.IsNullOrEmpty ( spacename ) )
			{
				return CreateXmlElement ( name, value );
			}
			if ( !string.IsNullOrEmpty ( name ) )
			{
				XNamespace xmlNamespace = CreateXNamespace ( spacename );
				if ( null == value )
				{
					return new XElement ( xmlNamespace + name );
				} else
				{
					return new XElement ( xmlNamespace + name );
				}
			}
			return null;
		}

		public static XNamespace CreateXNamespace ( string spacename )
		{
			XNamespace xmlNamespace;
			if ( spacename == "xmlns" )
			{
				xmlNamespace = XNamespace.Xmlns;
			} else if ( spacename == "xml" )
			{
				xmlNamespace = XNamespace.Xml;
			} else
			{
				xmlNamespace = spacename;
			}
			return xmlNamespace;
		}
	}
}
