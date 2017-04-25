using System;
using System.Data;
using Summer.System.Util;
using Summer.System.Util.Type;
using System.Data.SqlClient;

namespace Summer.System.Data.DbPump
{
    public abstract class BaseDbPump : IDbPump
    {
        protected BaseDbPump()
        {
            LastOperationError = new EventIndicator();
        }

        public IDbConnection Conn { get; set; }
        public IDbDataAdapter Adapter { get; set; }


        public void Open()
        {
            if (Conn != null)
                //conn.ConnectionString = ConnectionString;
                if (Conn.State != ConnectionState.Open)
                { 
                    Conn.Open(); 
                    Conn.Open(); 
                }

        }

        public void Close()
        {
            if (Conn != null)
                Conn.Close();
        }

        public bool IsOpened
        {
            get { return Conn != null && Conn.State == ConnectionState.Open; }
        }

        public EventIndicator LastOperationError { get; private set; }

        private object exeNonQuryLocker = new object();
        public int ExecuteNonQuery(string sql, bool isTrans)
        {
            //return Execute(sql, isTrans, cmm=> 1);
            int k=0;
            lock (exeNonQuryLocker)
            { 
                k=Execute(sql, isTrans, cmm => cmm.ExecuteNonQuery()); 
            }
            return k;
        }

        public object ExecuteScalar(string sql, bool isTrans)
        {
            return Execute(sql, isTrans, cmm => cmm.ExecuteScalar());
        }

        public DataSet ExecuteDataSet(string sql, bool isTrans)
        {
            return Execute(sql, isTrans, cmm =>
                {
                    DataSet ds = new DataSet();
                    Adapter.SelectCommand = cmm;
                    Adapter.Fill(ds);
                    return ds;
                });
        }





        private T Execute<T>(string sql, bool isTrans, Func<IDbCommand, T> func)
        {
            LastOperationError.Reset();
            T k = default(T);
            if (Conn == null)
            {
                LastOperationError.Happen("Connection为空");
                return k;
            }
            if (!IsOpened)
            {
                Conn.Open();
            }
            using (IDbCommand cmm = Conn.CreateCommand())
            {
                cmm.CommandText = sql;
                IDbTransaction trans = null;
                if (isTrans)
                {
                    trans = Conn.BeginTransaction();
                    cmm.Transaction = trans;
                }
                try
                {
                    k = func(cmm);
                    if (isTrans)
                        trans.Commit();
                }
                catch (ArgumentException e)
                {
                    Console.Out.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]")+sql + e.Message);
                }
                catch (InvalidOperationException e)
                {
                    Console.Out.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]")+sql + e.Message);
                }
                catch (SqlException e)
                {
                    Console.Out.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") +sql+ e.Message);
                }
                catch (Exception e)
                {
                    var str = ExceptionHelper.GetDetailMessage(e);
                    LastOperationError.Happen(str);
                    if (isTrans)
                        trans.Rollback();
                }
                finally
                {
                    if (isTrans)
                        trans.Dispose();
                    	//	Conn.Close ( );//不CLOSE,    请自己CLOSE
                }
            }

            return k;
        }



    }
}