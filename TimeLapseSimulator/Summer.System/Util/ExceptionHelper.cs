using System;
using System.Collections.Generic;

namespace Summer.System.Util
{
	public class ExceptionHelper
	{
		public static void GetDetailMessage ( ref List<string> rst, Exception e )
		{
			if ( e != null && rst != null )
			{
				rst.Add ( e.Message );
				GetDetailMessage ( ref rst, e.InnerException );
			}
		}
		public static string GetDetailMessage ( Exception e )
		{
			string rr = "";
			List<string> list = new List<string> ( );
			GetDetailMessage ( ref list, e );
			foreach (string t in list)
			{
				rr += t + "; ";
			}
			return rr;
		}
	}
}