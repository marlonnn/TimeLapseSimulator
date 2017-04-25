using System.Data.SQLite;

namespace Summer.System.Data.VarietyDb
{
 public   class SqliteConvertor : SqlServerConvertor
    {
        protected override global::System.Data.IDataParameter CreateDataParameter(string name, object value)
        {
            return new SQLiteParameter(name, value);
        }

    }
}
