using System;

namespace Summer.System.Security
{
	public class CRC32
	{
		public uint crcInit = 0;
		//public static uint poly = 0X04c11db7;
		public uint poly = 0x1EDC6F41;
		private uint[ ] CrcTable;

		/// <summary>
		/// DIRECT TABLE直驱表法，需要用到“直接查询表”,在windows中,小端
		/// </summary>
		public uint Crc ( byte[ ] buf, int len )
		{
			if ( buf == null )
			{
				throw new ArgumentNullException ( "buf is null" );
			}

			if ( len < 0 )
			{
				throw new ArgumentOutOfRangeException ( "off<0 or len<0 or off+len>buf.Length" );
			}

			UInt32 crc = crcInit;
			for ( int i = 0; i < len; ++i )
			{
				crc = ( crc << 8 ) ^ CrcTable[ ( crc >> 24 ) ^ buf[ i ] ];
			}

			return crc;
		}

		#region generate CRC direct table
		public void GenCRCTable ( )
		{
			UInt32 i32, j32;
			UInt32 nData32;
			UInt32 nAccum32;
			uint[ ] CrcTable11 = new uint[ 256 ];
			for ( i32 = 0; i32 < 256; i32++ )
			{
				nData32 = (UInt32) ( i32 << 24 );
				nAccum32 = 0;
				for ( j32 = 0; j32 < 8; j32++ )
				{
					UInt32 iiiii = ( ( nData32 ^ nAccum32 ) & 0x80000000 );
					if ( iiiii != 0x00 )
						nAccum32 = ( nAccum32 << 1 ) ^ poly;
					else
						nAccum32 <<= 1;
					nData32 <<= 1;
				}
				CrcTable11[ i32 ] = nAccum32;
			}
			CrcTable = CrcTable11;

		}
		#endregion
	}
}
