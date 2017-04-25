using System.Collections.Generic;
using System.Text;

namespace Summer.System.Util.StreamCreator
{
	class ValueConvertor
	{
		private const string str_tag = "str:";
		private const string hex_tag = "0x";

		public List<bool> Convert ( string raw_value, long bit_length, MessageConfig config )
		{
			raw_value = raw_value.Trim ( );
			byte[ ] rslt_bytes = new byte[ 0 ];
			if ( raw_value.StartsWith ( str_tag, true, null ) )
			{
				var str_value = raw_value.Substring ( str_tag.Length, raw_value.Length - str_tag.Length );
				rslt_bytes = Encoding.Default.GetBytes ( str_value );
			} else if ( raw_value.StartsWith ( hex_tag, true, null ) )
			{
				var hex_value = raw_value.Substring ( hex_tag.Length, raw_value.Length - hex_tag.Length );
				rslt_bytes = ByteHelper.Xstring2Byte ( hex_value );
			} else
			{
				long rr;
				long.TryParse ( raw_value, out rr );
				rslt_bytes = ByteHelper.longTobyte ( rr,config.IsBigEndien );
			}
			var rst_bits = byteTobits ( rslt_bytes, bit_length );

			return rst_bits;
		}

		private static List<bool> byteTobits ( IList<byte> bytes, long bit_length )
		{
			List<bool> rst_bits = new List<bool> ( );
			//倒着
			for ( int i = bytes.Count - 1; i >= 0 && rst_bits.Count < bit_length; i-- )
			{
				var by = bytes[ i ];
				for ( int j = 0; j < 8 && rst_bits.Count < bit_length; j++ )
				{
					var by1 = by >> j;
					rst_bits.Insert ( 0, ( by1 & 0x01 ) == 0x01 );
				}
			}
			return rst_bits;
		}
	}
}
