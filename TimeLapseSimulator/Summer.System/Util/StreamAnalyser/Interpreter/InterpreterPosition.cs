using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summer.System.Util.StreamAnalyser.Interpreter
{
	public class InterpreterPosition : ICloneable
	{
		public int index { get; set; }
		public int restbit { get; set; }

		public InterpreterPosition ( )
		{
			index = 0;
			restbit = 8;
		}

		public InterpreterPosition ( InterpreterPosition pos )
		{
			index = pos.index;
			restbit = pos.restbit;
		}
		/// <summary>
		/// 使pos的index+1
		/// </summary>
		/// <param name="step"></param>
		public void StepForward ( int step = 1 )
		{
			index += step;
		}

		public void StepForwardByBit ( int stepbit = 1 )
		{
			restbit -= stepbit % 8;
			index += stepbit / 8;
			if ( restbit <= 0 )
			{
				restbit += 8;
				index++;
			}
		}

		public object Clone ( )
		{
			return this.MemberwiseClone ( );
		}
	}
}
