using System;

namespace Summer.System.Util
{
	public class MathHelper
	{
		/// <summary>
		/// [修订: 戴唯艺(2014-02-20)]
		/// double类型数据相等判断, delta表示可允许误差,默认0.001(不包含),delta将被取绝对值后应用在函数中
		/// <para>src基准数; tar去比较的数</para>
		/// </summary>
		public static bool DoubleEqual ( double src, double tar,double delta=0.001 )
		{
			var d = Math.Abs(delta);
			return tar > src - d && tar < src + d;
		}
	}
}
