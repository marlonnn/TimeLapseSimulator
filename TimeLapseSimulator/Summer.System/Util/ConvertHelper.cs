using System;

namespace Summer.System.Util
{
	public class ConvertHelper
	{
		/// <summary>
		/// 若转换失败，则返回0
		/// </summary>
		public static UInt32 TryString2UINT32 ( string data )
		{
			UInt32 result;
			UInt32.TryParse ( data, out result );
			return result;
		} 
	}
}