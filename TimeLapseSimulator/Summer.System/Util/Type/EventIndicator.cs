namespace Summer.System.Util.Type
{
	public class EventIndicator
	{
		public bool IsHappend { get; set; }
		public string Msg { get; set; }
		public object Data { get; set; }

		public virtual void Reset()
		{
			IsHappend = false;
			Msg = "";
			Data = null;
		}

		public virtual void Happen(string msg)
		{
			IsHappend = true;
			Msg = msg;
		}
	}

	public class KeyWordIndicator : EventIndicator
	{
		public string Key { get; set; }
	}
}