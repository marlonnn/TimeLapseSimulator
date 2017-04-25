using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util;
using Summer.System.IO;
using Summer.System.Util.StreamAnalyser.Function;
using Summer.System.Util.StreamAnalyser.Protocal;

namespace Summer.System.Util.StreamAnalyser.Value
{
	/// <summary>
	/// 数据值类
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：张广宇
	/// 创建日期：2013-7-14
	/// </remark>
	public class TermValue
	{
		/// <summary>
		/// 以字符串形式保存数据值
		/// </summary>
		public string value { get; protected set; }

		/// <summary>
		/// 根据字符串生成数据项值类
		/// </summary>
		/// <param name="val"></param>
		public TermValue ( string val )
		{
			if ( val.StartsWith ( "0x" ) || val.StartsWith ( "0X" ) )
			{//如果Value为0X格式则视作16进制数据，需转换为10进制数据存储
				long num = 0;
				if ( Int64.TryParse ( val.Substring ( 2 ), NumberStyles.HexNumber, null, out num ) )
				{
					SetValue ( num );
				} else
				{
					SetValue ( val );
				}
			} else
			{
				SetValue ( val );
			}
		}

		/// <summary>
		/// 根据长整形数生成数据项值类
		/// </summary>
		/// <param name="val"></param>
		public TermValue ( long val )
		{
			SetValue ( val );
		}

		/// <summary>
		/// 根据byte数组生成数据项值类
		/// </summary>
		/// <param name="val"></param>
		public TermValue ( byte[ ] val )
		{
			SetValue ( val );
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="val"></param>
		public TermValue ( TermValue val )
		{
			SetValue ( val );
		}

		public void SetValue ( string val )
		{
			value = val;
		}

		public virtual void SetValue ( long val )
		{
			value = val.ToString ( );
		}

		public void SetValue ( byte[ ] val )
		{
			value = ByteHelper.Byte2Xstring ( val ).ToUpper ( );
		}

		public void SetValue ( TermValue val )
		{
			value = val.value;
		}

		/// <summary>
		/// 是否为变量
		/// </summary>
		/// <returns></returns>
		public virtual bool IsVariant ( )
		{
			return false;
		}

		public virtual TermValueRef Variant ( )
		{
			return null;
		}

		/// <summary>
		/// 赋值操作
		/// </summary>
		protected virtual void Assign ( )
		{
		}

		/// <summary>
		/// 复制生成TermValue对象
		/// </summary>
		/// <returns></returns>
		public virtual TermValue Clone ( )
		{
			return new TermValue ( this );
		}

		/// <summary>
		/// 整合函数，用以关联协议的索引，在协议类全部生成后调用
		/// </summary>
		/// <param name="eflist"></param>
		/// <param name="pt"></param>
		public virtual void Intergation ( List<ExpressionFunc> eflist, ProtocalTerm pt )
		{
		}

		/// <summary>
		/// 格式化输出数值
		/// </summary>
		/// <returns></returns>
		public override string ToString ( )
		{
			return value;
		}

		/// <summary>
		/// 格式化输出Int32
		/// </summary>
		/// <remarks>
		/// 作者：戴唯艺
		/// 日期：2014-5-4
		/// </remarks>
		public virtual int toInt32 ( )
		{
			int rst;
			Int32.TryParse ( value, out rst );
			return rst;
		}

		/// <summary>
		/// 重写Equals相等函数，用来替代operator==和operator!=
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals ( object obj )
		{
			if ( obj == null )
			{
				return false;
			}
			if ( obj is TermValue )
			{
				if ( value == ( obj as TermValue ).value )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 重写GetHashCode函数
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode ( )
		{
			return value.GetHashCode ( );
		}

		/// <summary>
		/// 根据XML片段生成TerValue对象,包括TermValue、TermValueRef、TermValueExpression
		/// </summary>
		/// <param name="xmlVisitor"></param>
		/// <param name="tv"></param>
		/// <returns></returns>
		static public TermValue Create ( XmlVisitor xmlVisitor, TermValue tv )
		{
			TermValue termvalue = tv;
			do
			{
				if ( null == xmlVisitor )
				{
					break;
				}
				string value = xmlVisitor.GetAttribute ( "value" );
				if ( !string.IsNullOrEmpty ( value ) )
				{
					termvalue = new TermValue ( value );
					break;
				}
				string valueref = xmlVisitor.GetAttribute ( "valueref" );
				if ( !string.IsNullOrEmpty ( valueref ) )
				{
					termvalue = new TermValueRef ( valueref );
					break;
				}
				string func = xmlVisitor.GetAttribute ( "func" );
				if ( !string.IsNullOrEmpty ( func ) )
				{
					List<TermValue> paramlist = new List<TermValue> ( );
					foreach ( XmlVisitor xmlChild in xmlVisitor.FilterChildren ( "param" ) )
					{
						TermValue param = Create ( xmlChild, null );
						paramlist.Add ( param );
					}
					termvalue = new TermValueExpression ( func, tv, paramlist );
				}
			}
			while ( false );
			return termvalue;
		}

		/// <summary>
		/// 强制转换为string，未处理底层异常
		/// </summary>
		/// <param name="tv">TermValue</param>
		/// <returns></returns>
		static public explicit operator string ( TermValue tv )
		{
			tv.Assign ( );
			return tv.value;
		}

		/// <summary>
		/// 强制转换为long，未处理底层异常
		/// </summary>
		/// <param name="tv">TermValue</param>
		/// <returns></returns>
		static public explicit operator long ( TermValue tv )
		{
			tv.Assign ( );
			return Convert.ToInt64 ( tv.value );
		}

		/// <summary>
		/// 强制转换为bytes，未处理底层异常
		/// </summary>
		/// <param name="tv">TermValue</param>
		/// <returns></returns>
		static public explicit operator byte[ ] ( TermValue tv )
		{
			tv.Assign ( );
			return ByteHelper.Xstring2Byte ( tv.value );
		}
	}
}
