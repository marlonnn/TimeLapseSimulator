using System.Net;

namespace Summer.System.MVVM
{
	public class MVVMHelper
	{
		public static string GetString ( object[ ] p, int indx )
		{
			if ( p.Length <= indx )
				return "";
			return p[ indx ] as string;
		}
		public static byte[ ] GetBytes ( object[ ] p, int indx )
		{
			if ( p.Length <= indx )
				return new byte[ 0 ];
			return p[ indx ] as byte[ ];
		}
		public static IPEndPoint GetIPEndPoint ( object[ ] p, int indx )
		{
			if ( p.Length <= indx )
				return new IPEndPoint ( IPAddress.Any, 8080 );
			return p[ indx ] as IPEndPoint;
		}
	}
}