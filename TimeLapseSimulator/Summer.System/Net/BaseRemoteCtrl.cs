using System;
using Summer.System.Collections.Concurrent;

namespace Summer.System.NET
{
	public abstract class BaseRemoteCtrl<T> : IRemoteControler<T>
	{
		public IPump pump;
		protected ISerializor<T> serializor;
		protected IFilter filter;
		//private readonly BlockingCollection<T> msgReceiveQueue = new BlockingCollection<T> ( new global::System.Collections.Concurrent.ConcurrentQueue<T> ( ) );
		//private CancellationTokenSource ctsTake;

		private readonly BlockingConcurrentQueue<T> msgRxQueue = new BlockingConcurrentQueue<T> ( );


		public bool IsRunning
		{
			get { return pump != null && pump.IsOpened; }
		}

		public virtual void Init ( )
		{
			if ( pump != null )
				pump.DataReceived += pump_DataReceived;
			if ( filter != null )
				filter.DataFiltered += filter_DataFiltered;
			if ( serializor != null )
				serializor.SomethingHappend += serializor_SomethingHappend;
		}

		void serializor_SomethingHappend ( object sender, SEventArgs<object> e )
		{
			RaiseSomethingHappend ( sender, e );
		}

		void filter_DataFiltered ( object sender, SEventArgs<byte[ ]> e )
		{
			var bytes = e.Value;

			var msg = default ( T );
			if ( serializor != null )
				msg = serializor.Deserialize ( bytes );
			//就算是null值,也添加
			msgRxQueue.Add ( msg );
			//msgReceiveQueue.Add ( msg );
			//有可能为值类型,
			//if ( !msg.Equals ( default ( T ) ) )
			//{
			//    msgReceiveQueue.Add ( msg );
			//}
		}

		void pump_DataReceived ( object sender, SEventArgs<byte[ ]> e )
		{
			if ( filter != null )
				filter.Filter ( e.Value );
		}

		public void Run ( )
		{
			if ( pump.IsOpened )
				return;
			msgRxQueue.Start ( );
			//ctsTake = new CancellationTokenSource ( );
			pump.Open ( );
		}

		public void Stop ( )
		{
			pump.Close ( );
			msgRxQueue.Cancel ( );
			//if ( ctsTake != null )
			//    ctsTake.Cancel ( );
		}

		public void Send ( T msg )
		{
			var bytes = new byte[ 0 ];
			if ( serializor != null )
				bytes = serializor.Serialize ( msg );

			pump.Send ( bytes );
		}

		/// <summary>
		/// 最好新开一个线程,来接收,若操作被取消，则返回false
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool BlockingFetch ( out T msg )
		{
			return msgRxQueue.Take ( out msg );
			//bool result = true;
			//msg = default ( T );
			//try
			//{
			//    msg = msgReceiveQueue.Take ( ctsTake.Token );
			//} catch ( OperationCanceledException e )
			//{
			//    result = false;
			//}
			//return result;
		}

		public event EventHandler<SEventArgs<object>> SomethingHappend;
		protected void RaiseSomethingHappend ( object sender, SEventArgs<object> e )
		{
			if ( SomethingHappend != null )
				SomethingHappend ( sender, e );
		}
	}

}