using System;

namespace Summer.System.Util.StreamAnalyser.Interpreter
{
	/// <summary>
	/// 说明：
	/// 1、抛出异常，里面包含一个Log信息
	/// 2、使用时 最好Clone一个Log类，而不是直接把Log类放入，毕竟Log类是单态类。
	/// Note：
	/// 1、该Exception是为了配合解析程序中的Exception而作
	/// 2、不过除了解析，其它地方的Exception不能保证替换
	/// </summary>
	/// <remarks>
	/// 作者：戴唯艺
	/// 时间：2013-0
	/// </remarks>
	public class InterpreterException : Exception
	{
		public InterpreterException ( InterpretLog _interpretLog )
		{
			this.InterpretLog = _interpretLog;
		}
		public InterpretLog InterpretLog { get; private set; }

	}
}