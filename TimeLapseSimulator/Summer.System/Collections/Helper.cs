using System.Collections.Generic;
using System.Threading;

namespace Summer.System.Collections
{
	public static class Helper
	{
		public static bool TryDequeueNoneBlock<T> ( this global::System.Collections.Concurrent.ConcurrentQueue<T> queue, out T result,
												int waittime = 1000 )
		{
			bool r = true;
			if ( !queue.TryDequeue ( out result ) )
			{
				Thread.Sleep ( waittime );
				r = false;
			}
			return r;
		}

		public static Tv GetDictionaryValue<Tk, Tv> (this IDictionary<Tk, Tv> dic, Tk key, Tv defaultvalue = default(Tv) )
		{
			if ( dic != null && dic.ContainsKey ( key ) )
				return dic[ key ];
			return defaultvalue;
		}
	}

}
