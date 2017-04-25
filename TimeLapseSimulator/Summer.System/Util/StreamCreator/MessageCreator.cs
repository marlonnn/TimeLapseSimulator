using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Util.StreamAnalyser.Protocal;

namespace Summer.System.Util.StreamCreator
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 公司：CASCO
	/// 作者：戴唯艺
	/// 创建日期：2013-5-1
	/// </remarks>
	class MessageCreator
	{
		private ProtocalDesigner pd;
		public MessageCreator(ProtocalDesigner pd)
		{
			this.pd = pd;
		}

		public List<bool> CreateBits(string frame_name, MessageConfig config)
		{
			var bits = new List<bool>();
			var frame_designer = pd.ProtocalTermList.Find(x => x.Name == frame_name);
			if (frame_designer != null)
				bits.AddRange(frame_designer.CreateBits(config));
			return bits;
		}
	}
}
