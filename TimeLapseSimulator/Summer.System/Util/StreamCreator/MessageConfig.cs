using System.Collections.Generic;
using Summer.System.Util.StreamAnalyser.Protocal;

namespace Summer.System.Util.StreamCreator
{
	class MessageConfig
	{
		private readonly Dictionary<string, string> fullname_rawvalue_mapping = new Dictionary<string, string> ( );
		private readonly ValueConvertor convertor = new ValueConvertor ( );

		public bool IsBigEndien = false;

		public List<bool> GetBits ( ProtocalTerm p_term )
		{
			//todo：需要考虑Occurs的情况
			string raw_value;
			fullname_rawvalue_mapping.TryGetValue ( p_term.FullName, out raw_value );
			var bits = convertor.Convert ( raw_value, p_term.Interpreter.LengthByBit, this );
			return bits;
		}
	}
}
