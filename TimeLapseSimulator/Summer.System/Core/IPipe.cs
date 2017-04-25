namespace Summer.System.Core
{
	public interface IPipe
	{
		/// <summary>
		/// 注意异常捕获
		/// </summary>
		void Open ( );
		void Close ( );
		bool IsOpened { get; }
	}
}
