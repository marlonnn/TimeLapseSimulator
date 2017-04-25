using System;
using System.Collections.Generic;
using System.Linq;

namespace Summer.System.Util.StreamAnalyser.Message
{
	/// <summary>
	/// 作者:戴唯艺 
	/// </summary>
	public static class MsgHelper
	{
		public static string GetExplain ( this MessageTermSlot mts )
		{
			return mts.PT.Explain;
		}
		public static string GetMeaning ( this MessageTermSlot mts )
		{
			return mts.PT.Meaning.value;
		}
		public static Byte GetByteValue ( this MessageTermSlot mts )
		{
			Byte by;
			Byte.TryParse ( mts.Value.value, out by );
			return by;
		}
		public static UInt16 GetUInt16Value ( this MessageTermSlot mts )
		{
			return Convert.ToUInt16 ( mts.Value.value );
		}

		public static MessageTermComplex AsMessageTermComplex ( this MessageTermSlot mts )
		{
			return mts as MessageTermComplex;
		}

		public static List<MessageTermSlot> GetMessageTermSlotList ( this MessageTermSlot mts )
		{
			MessageTermComplex mtc = mts.AsMessageTermComplex ( );
			return mtc != null ? mtc.MessageTermList : new List<MessageTermSlot> ( );
		}

		public static MessageTermSlot GetMessageTermSlotFirst ( this MessageTermSlot mts )
		{
			MessageTermComplex k = mts.AsMessageTermComplex ( );
			return k != null ? k.MessageTermList.FirstOrDefault ( ) : null;
		}

		public static string GetValue ( this MessageTermSlot mts )
		{
			if ( mts.Value == null )
				return "";
			return mts.Value.value;
		}

		public static bool GetBoolValue ( this MessageTermSlot mts )
		{
			var str = GetValue ( mts );
			return str == "1";
		}

		public static UInt32 GetUInt32Value ( this MessageTermSlot mts )
		{
			return Convert.ToUInt32 ( mts.Value.value );
		}
	}
}
