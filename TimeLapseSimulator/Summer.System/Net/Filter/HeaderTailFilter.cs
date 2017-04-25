using System;
using System.Collections.Generic;
using Summer.System.Util;

namespace Summer.System.NET.Filter
{
	public class HeaderTailFilter : IFilter
	{
		public readonly Matcher Header = new Matcher ( 0x00 );
		public readonly Matcher Tail = new Matcher ( 0x00 );
		public int MaxPostfix = 0;

		private bool isFilling;
		private bool atTail;

		private int CurrPostFix = 0;



		/// <summary>
		/// 没有输出的byte放在此
		/// </summary>
		private readonly List<byte> byteForLog = new List<byte> ( );
		//只是切片
		

		public void Filter ( byte[ ] streamBytes )
		{
			foreach ( byte currentByte in streamBytes )
			{
				//是否正在填充
				if ( !isFilling )
				{
					//没有填充时
					var rst = Header.Proceed ( currentByte );

					if ( rst == MatchState.Match )
					{
						//找到帧头,开始填充,
						isFilling = true;
						foreach ( byte t in Header.CurrentMatchBuffer )
						{//填好帧头
							byteForLog.Add ( t );
						}
					}
				} else
				{
					byteForLog.Add ( currentByte );

					if ( byteForLog.Count > 65535 )
						byteForLog.Clear ( );//超量防护,自动舍弃
					//正在填充时

					if ( !atTail )
					{	//若没有读到帧尾，继续往下读；
						var rst = Tail.Proceed ( currentByte );
						if ( rst == MatchState.Match )
						{
							//表示读到帧尾
							atTail = true;
							if ( MaxPostfix == 0 )//如果不用多读
								Okbytes ( );
						}
					} else
					{	//若读到帧尾，需要多读,再读n次,停止读取数据，
						CurrPostFix++;
						if ( CurrPostFix >= MaxPostfix )
							Okbytes ( );
					}
				}
			}
		}

		private void Okbytes ( )
		{
			//表示可以截取了
			if ( DataFiltered != null )
			{
				var evt = new SEventArgs<byte[ ]> ( "Filtered", byteForLog.ToArray ( ) );
				DataFiltered.Invoke ( this, evt );
			}
			byteForLog.Clear ( );
			isFilling = false;
			CurrPostFix = 0;
			atTail = false;
		}


		public event EventHandler<SEventArgs<byte[ ]>> DataFiltered;
	}
}