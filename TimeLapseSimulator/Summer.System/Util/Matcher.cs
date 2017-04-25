using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Summer.System.Util
{
	public enum MatchState
	{
		Match,
		NotMatch,
		Matching,
		/// <summary>
		/// 在匹配队列中,一个都不匹配
		/// </summary>
		NotMatchAll,
		OneMatch,
		/// <summary>
		/// 在匹配队列中,存在正在匹配的组合
		/// </summary>
		SomeMatching
	}

	public class Matcher
	{
		public MatchState State { get; private set; }
		private readonly Dictionary<MatchState, Func<byte, MatchState>> stateChanger;

	

		public Matcher ( params byte[ ] bytes )
			: this ( )
		{
			if ( bytes == null || bytes.Length == 0 )
				throw new ArgumentException ( "bytes长度至少需为1" );
			target = bytes;
		}

		public Matcher ( )
		{
			stateChanger = new Dictionary<MatchState, Func<byte, MatchState>> ( );
			stateChanger[ MatchState.NotMatch ] = Proceed_Autorest_NotMatch;
			stateChanger[ MatchState.Match ] = Proceed_Autorest_NotMatch;
			stateChanger[ MatchState.Matching ] = Proceed_Matching;

			State = MatchState.NotMatch;

		}

		public  byte[ ] target;
		private int index = 0;

		public void Reset ( )
		{
			index = 0;
			State = MatchState.NotMatch;
		}

		private MatchState Proceed_Autorest_NotMatch ( byte b )
		{
			//自动重置
			Reset ( );
			if ( b == target[ index ] )
			{
				index++;
				State = target.Length == index ? MatchState.Match : MatchState.Matching;
			} else
				State = MatchState.NotMatch;

			return State;
		}

		private MatchState Proceed_Matching ( byte b )
		{
			Trace.Assert ( index > 0 );
			if ( target[ index ] == b )
			{
				index++;
				if ( index == target.Length )
					State = MatchState.Match;
			} else
				State = MatchState.NotMatch;

			return State;
		}

		public MatchState Proceed ( byte b )
		{
			return stateChanger[ State ] ( b );
		}

		/// <summary>
		/// 不一定包含所有Target数据
		/// </summary>
		public List<byte> CurrentMatchBuffer
		{
			get
			{
				List<byte> rst = new List<byte> ( );
				for ( int i = 0; i < CrntMatchBufferLength; i++ )
				{
					rst.Add ( target[ i ] );
				}
				return rst;
			}
		}

		public int CrntMatchBufferLength
		{
			get {
				return index < target.Length ? index : target.Length;
			}
		}
	}
}
