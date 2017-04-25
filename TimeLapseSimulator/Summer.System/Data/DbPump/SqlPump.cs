using System.Data.SqlClient;

namespace Summer.System.Data.DbPump
{
	public class SqlPump : BaseDbPump
	{
		public SqlPump ( )
		{
			Conn = new SqlConnection ( );
			Adapter = new SqlDataAdapter ( );
		}
	}
}
