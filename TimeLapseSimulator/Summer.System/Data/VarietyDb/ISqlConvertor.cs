using System.Collections.Generic;
using Spring.Data.Common;
using Summer.System.Data.DbMapping;

namespace Summer.System.Data.VarietyDb
{

	/// <summary>
	/// Sql转换器，参数配置工具。转换时确保能够正确描述参数位，（例如MsSqlServer需要@表示参数位）
	/// </summary>
	/// <remark>
	/// 公司：CASCO
	/// 作者：戴唯艺                 
	/// 创建日期：2013-5-23   
	/// </remark>
	public interface ISqlConvertor
	{
		string MakeInsertSql ( string tableName, List<string> dbFieldList );
		string MakeUpdateSql ( string tableName, List<string> dbFieldListWithoutKey, List<string> dbKeyFieldList );
		string MakeDeleteSql ( string tableName, List<string> dbKeyFieldList );
		string MakeSelectSql ( string tableName, List<string> dbKeyFieldList );

	  
	    DbParameters CreateParameters (Dictionary<string, FieldAttribute> dbFieldDef, Dictionary<string, object> classValue);

		string paramCreator(string field);
	}

}