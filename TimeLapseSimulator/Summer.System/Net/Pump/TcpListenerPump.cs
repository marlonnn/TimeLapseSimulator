using Summer.System.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Summer.System.NET.Pump
{
	public class TcpListenerPump : IPump
	{
		public readonly TcpListener Listener = new TcpListener ( IPAddress.Any, 8080 );
		/// <summary>
		/// 接收到的client端
		/// </summary>
		private TcpClient client;
		private NetworkStream ns;

		public bool ChangePort ( int port )
		{
			if ( IsOpened )
				Close ( );
			var ip = Listener.LocalEndpoint as IPEndPoint;
			ip.Port = port;
			return true;
		}

		/// <summary>
		/// 通过Open建立连接,只仅
		/// </summary>
		public void Open ( )
		{
			Listener.Start ( );

			Task.Factory.StartNew ( ( ) =>
			{
				try
				{
					Receive ( );
				} catch (SocketException e )
				{//一般来说中断/关闭监听,会引发此异常
                    LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
                    ;
				}
			}
				);
		}

		private void Receive ( )
		{
			client = Listener.AcceptTcpClient ( );
			ns = client.GetStream ( );

			while ( true )
			{
				var len = client.Available > 1 ? client.Available : 1;
				var bytes = new byte[ len ];

				int rr;
				//至少有一个有效数据,才能消除阻塞
				try
				{
					rr = ns.Read ( bytes, 0, len );
				} catch ( Exception e )
				{
                    //关闭自身client引发异常
                    LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
                    break;
				}
				if ( rr == 0 )
				{
					//"因为远端clinet被关闭,停止接收" 
					//"等待新Client
					client = Listener.AcceptTcpClient ( );
					ns = client.GetStream ( );
				}
				if ( DataReceived != null )
					DataReceived ( this, new SEventArgs<byte[ ]> ( "TcpListener", bytes ) );
			}
		}

		public void Send ( byte[ ] bytes )
		{
			try
			{
				ns.Write ( bytes, 0, bytes.Length );
			} catch ( Exception e )
			{
                LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
                ;
			}
		}

		public void Close ( )
		{
			if ( client != null )
				client.Close ( );
			Listener.Stop ( );
		}

		public bool IsOpened
		{//不要用connected判断
			get { return Listener.Server.IsBound; }
		}
		public event EventHandler<SEventArgs<byte[ ]>> DataReceived;
	}
}