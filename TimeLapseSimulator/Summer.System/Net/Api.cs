using System;
using Summer.System.Core;

namespace Summer.System.NET
{
	public interface IRemoteControler<T>
	{
		bool IsRunning { get; }
		void Init ( );
		void Run ( );
		void Stop ( );
		void Send ( T msg );

		/// <summary>
		/// 阻塞方式取数据
		/// </summary>
		bool BlockingFetch ( out T msg );

		event EventHandler<SEventArgs<object>> SomethingHappend;
	}

	public class SEventArgs<T> : EventArgs
	{
		public SEventArgs ( string key, T value )
		{
			Key = key;
			Value = value;
		}
		public string Key { get; private set; }
		public T Value { get; private set; }
	}

	public interface IPump:IPipe
	{
		/// <summary>
		/// 注意异常捕获
		/// </summary>
		//void Open ( );
		void Send ( byte[ ] bytes );

		//void Close ( );
		//bool IsOpened { get; }
		event EventHandler<SEventArgs<byte[ ]>> DataReceived;
	}

	public interface ISerializor<Tapp>
	{
		Tapp Deserialize ( byte[] persist );
		byte[] Serialize ( Tapp app );
		event EventHandler<SEventArgs<object>> SomethingHappend;
	}

	public interface IFilter
	{
		
		//过滤后将发送消息通知下一步处理
		void Filter ( byte[ ] streamBytes );
		event EventHandler<SEventArgs<byte[]>> DataFiltered;
	}
}