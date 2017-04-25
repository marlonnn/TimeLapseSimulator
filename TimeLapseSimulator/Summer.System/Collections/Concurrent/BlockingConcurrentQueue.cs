using Summer.System.Log;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Summer.System.Collections.Concurrent
{
	public class BlockingConcurrentQueue<T>
	{
		private readonly BlockingCollection<T> bcQueue = new BlockingCollection<T> ( new global::System.Collections.Concurrent.ConcurrentQueue<T> ( ) );
		private CancellationTokenSource cts;

		public bool IsRunning { get { return cts != null && !cts.IsCancellationRequested; } }
		public void Start ( )
		{
			if ( !IsRunning )
				cts = new CancellationTokenSource ( );
		}

		public void Cancel ( )
		{
			if ( cts != null )
				cts.Cancel ( );
		}

		/// <summary>
		/// 如果操作被取消,则返回false
		/// </summary>
		public bool Take ( out T result )
		{
			result = default ( T );
			var rr = true;
			try
			{
				if ( IsRunning )
					result = bcQueue.Take ( cts.Token );
				else
					rr = false;
			} catch ( OperationCanceledException e )
			{
				rr = false;
                LogHelper.GetLogger<BlockingConcurrentQueue<T>>().Error(e.StackTrace);
			}
			return rr;
		}

		public void Add ( T item )
		{
			if ( IsRunning )
				bcQueue.Add ( item );
		}
	}
}