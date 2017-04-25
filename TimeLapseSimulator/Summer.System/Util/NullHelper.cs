using System.Collections.Generic;
using System.Linq;

namespace Summer.System.Util
{
	public class NullHelper
	{
		public static bool IsNullOrEmpty<T> ( IEnumerable<T> list )
		{
			if ( list == null )
				return true;
			if ( !list.Any ( ) )
				return true;
			return false;
		}
	}
}
