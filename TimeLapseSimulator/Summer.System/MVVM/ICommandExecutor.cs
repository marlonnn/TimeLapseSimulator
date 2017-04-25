namespace Summer.System.MVVM
{
	public interface ICommandExecutor
	{
		void ExexuteCommand(object src,string cmdType,params object[] cmdParams );
	}
}
