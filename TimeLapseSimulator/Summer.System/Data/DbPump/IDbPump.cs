using System.Data;
using Summer.System.Core;
using Summer.System.Util.Type;

namespace Summer.System.Data.DbPump
{
	public interface IDbPump : IPipe
	{
		IDbConnection Conn { get; set; }
		IDbDataAdapter Adapter { get; set; }

		EventIndicator LastOperationError { get; }

		int ExecuteNonQuery ( string sql, bool isTrans=false );
		object ExecuteScalar ( string sql, bool isTrans=false );
		DataSet ExecuteDataSet ( string sql, bool isTrans=false );

	}
}
