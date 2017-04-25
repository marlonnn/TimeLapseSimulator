using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Summer.System.Collections
{
	public class AsynchQueueWorker<T>
	{
		private readonly global::System.Collections.Concurrent.ConcurrentQueue<T> queue = new ConcurrentQueue<T> ( );
		private Task task;
		private volatile bool is_running = false;
		public Action<T> AfterGetValue { get; set; }
		private int wait_time = 1000;
		public int WaitTime { get { return wait_time; } set { wait_time = value; } }
        private int maxSize = int.MaxValue;
        public int MaxSize { get { return maxSize; } set { maxSize = value; } }
        /// <summary>
        /// 返回当前队列中元素个数
        /// </summary>
        public int Count { get { return queue.Count; } }
		public void Add ( T k )
		{
            if (is_running)
            {
                while (queue.Count > MaxSize)
                {
                    T r;
                    queue.TryDequeue(out r);
                    Console.Out.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] 队列溢出，丢弃旧数据"));
                }
                queue.Enqueue(k);
            }
		}

		public void Clear()
		{
			while (queue.Count>0)
			{
				T d;
				queue.TryDequeue(out d);
			}
		}

		private void run ( )
		{
			while ( is_running )
			{
				T args;
				if ( queue.TryDequeueNoneBlock ( out args, wait_time ) )
					if ( AfterGetValue != null )
						AfterGetValue ( args );
			}
		}
        private void taskEnd(Task t)
        {
            if (t.IsFaulted)
            {
                Start();//错误了，自动重启
            }
        }
		public void Start ( )
		{
			Stop ( );
			is_running = true;
            if(task!=null)
              task.Dispose();
			task = Task.Factory.StartNew ( run );
            task.ContinueWith(taskEnd);
		}

		public void Stop ( )
		{
			is_running = false;
			if ( task != null )
				task.Wait ( );
		}
	}
}