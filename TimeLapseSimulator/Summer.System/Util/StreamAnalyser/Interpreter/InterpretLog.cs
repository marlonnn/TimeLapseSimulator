using System;

namespace Summer.System.Util.StreamAnalyser.Interpreter
{
	/// <summary>
	/// 一些说明：
	/// 1、该类定位“数据取不出来了”的地点和原因
	/// 2、至于“取出来的数据是否正确”，请移至具体Model类定义中判断
	/// 3、可能是因为先前的“数据错位”而造成末尾的某数据取不出来，于是就算定位“数据取不出来”的地点，也只能得到最后的地点而不知道最初的起因地点
	/// 4、不能得到造成“数据错位”的起始地点，也是因为不能知道“取出来的数据是否正确”这个原因
	/// 5、多多在xml格式文件中定义某区段(frame、sequence)的Length能在一定程度上精确错误发生的记录地点。
	/// 6、如果多处发生“数据取不出来了”，则该类只会定位最近一次发生的地点
	/// 使用方法：
	/// 由于是单态类，故只需要在返回false的Interpret分支中取其实例即可
	/// </summary>
	/// <remarks>
	/// 作者：戴唯艺
	/// 时间：2013-09-03
	/// </remarks>
	public class InterpretLog : ICloneable
	{
		public const string OutOfRange = "解析结构需要解析数据，但是实际数据长度不够";
		public const string LongerThanBit64 = "每Protocal项最多只能解析64bit(8byte)的数据，尝试解析更长的数据，导致失败";
		public const string FramerNull = "虽为Frame引用，但是并没有引用什么frame";
		public const string FrameErrorInterept = "能寻找到Frame引用，但是解析出错";
		public const string ByteAlignError = "需要按照字节对齐，才能解析";
		private InterpretLog ( )
		{
		}

		private static InterpretLog _interpretLog;
		public static InterpretLog GetInstance ( )
		{
			return _interpretLog ?? ( _interpretLog = new InterpretLog ( ) );
		}
		public InterpreterPosition Pos { get; set; }
		public string Message { get; set; }
		public string FullName { get; set; }

		public object Clone ( )
		{
			var log1 = this.MemberwiseClone ( ) as InterpretLog;
			log1.Pos = Pos.Clone ( ) as InterpreterPosition;
			return log1;
		}
	}
}