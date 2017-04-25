using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Summer.System.Data.DbMapping
{
	/// <summary>
	/// 该类主要处理Database数据库字段和Class对象Property的各种映射关系
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：戴唯艺                 
	/// 创建日期：2013-5-23   
	/// </remark>
	public static class MappingHelper
	{
	


		/// <summary>
		/// 取得该Type对应的数据库表
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetTableName ( Type type )
		{
			object[ ] atts = type.GetCustomAttributes ( typeof ( TableAttribute ), false );
			return atts.OfType<TableAttribute> ( ).Select ( x => x.Name ).FirstOrDefault ( ) ?? "";
		}
		/// <summary>
		/// 取得类Property.Name和Database字段定义的对应关系,通过ProperName映射到Database
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Dictionary<string, FieldAttribute> Get_PropertyName_DbFieldDef ( Type type )
		{
			FieldInfo[ ] proInfos = type.GetFields ( );
			var proInfoDic = new Dictionary<string, FieldAttribute> ( );
			foreach ( var info in proInfos )
			{
				object[ ] atts = info.GetCustomAttributes ( typeof ( FieldAttribute ), false );
				FieldAttribute dbFieldInfo = atts.OfType<FieldAttribute> ( ).FirstOrDefault ( );
				if ( dbFieldInfo != null )
				{
					proInfoDic[ info.Name ] = dbFieldInfo;
				}
			}
			return proInfoDic;
		}
		/// <summary>
		/// 取得类Property.Name和Property.Value的对应关系
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		public static Dictionary<string, object> GetPropertyValue ( object record )
		{
			Type type = record.GetType ( );
			FieldInfo[ ] proInfos = type.GetFields ( );
			var propertyDic = new Dictionary<string, object> ( );
			foreach ( var info in proInfos )
			{

				object value = info.GetValue ( record );
				propertyDic[ info.Name ] = value;
			}
			return propertyDic;
		}
		/// <summary>
		/// 取得主键字段名
		/// </summary>
		public static List<string> GetKeyDbField ( Dictionary<string, FieldAttribute> dbFieldDef )
		{
			var result = new List<string> ( );
			foreach ( var caPair in dbFieldDef )
			{
				if ( caPair.Value.PrimaryKey )
					result.Add ( caPair.Value.Name );
			}
			return result;
		}
		/// <summary>
		/// 取得数据库字段名
		/// </summary>
		/// <param name="dbFieldDef"></param>
		/// <returns></returns>
		public static List<string> GetDbField ( Dictionary<string, FieldAttribute> dbFieldDef )
		{
			var result = new List<string> ( );
			foreach ( var caPair in dbFieldDef )
			{

				result.Add ( caPair.Value.Name );
			}
			return result;
		}
	}

}