using Summer.System.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Summer.System.NET.Pump
{
	public class TcpClientPp : IPump
	{
		private TcpClient client = new TcpClient ( ){NoDelay = true};

		public IPEndPoint RemoteEP { get; set; }
	//	private NetworkStream ns;
		public int AnsyncWaitTime = 100;
		public bool IsAnsyncConnect = false;
		private bool CanUseConnect = true;

		private void SyncConnect ( )
		{
			try
			{
				client.Connect ( RemoteEP );
			} catch ( Exception e )
			{
				//连接失败
				CanUseConnect = false;
                LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
			}
			CanUseConnect = true;
		}

		private void AnsyncConnect ( )
		{
			var ar = client.BeginConnect ( RemoteEP.Address, RemoteEP.Port, connect_complete, null );
			if ( ar.AsyncWaitHandle.WaitOne ( AnsyncWaitTime ) )
				CanUseConnect = true;
			else
			{
				CanUseConnect = false;
			}
		}


		private void connect_complete ( IAsyncResult ar )
		{
			try
			{
				client.EndConnect ( ar );
			} catch ( Exception e )
			{
				CanUseConnect = false;
                LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
            }
			;
		}


		public void Open ( )
		{
			//	client = new TcpClient ( );
			if ( IsAnsyncConnect )
				AnsyncConnect ( );
			else
				SyncConnect ( );

			if ( CanUseConnect )
				Task.Factory.StartNew ( ( ) =>
					{
						try
						{
							Receive ( );
						} catch ( SocketException e )
						{
                            //一般来说中断/关闭监听,会引发此异常
                            LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
						}
					}
					);

		}

		public Socket GetSocket ( )
		{
			return client.Client;
		}

		public string OpenType { get; set; }

		private void Receive ( )
		{
			
			//ns = client.GetStream ( );
			while ( true )
			{
				//至少需要读取1byte数据
				var len = client.Available > 1 ? client.Available : 1;
				var bytes = new byte[ len ];

				//至少有一个有效数据,才能消除阻塞
				try
				{
					client.Client.Receive(bytes);
					//ns.Read ( bytes, 0, len );
				} catch ( Exception e )
				{
                    //关闭自身client引发异常，关闭远端server
                    LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
                    break;
				}

				if ( DataReceived != null )
					DataReceived ( this, new SEventArgs<byte[ ]> ( "TcpClient", bytes ) );
			}
		}
		public void Close ( )
		{
			if (client.Connected)
			{
				client.Close ( );
				client=new TcpClient();
				client.NoDelay = true;
			}
		}

		public bool IsOpened
		{
			get
			{
				return CanUseConnect &&
					client != null &&
					client.Connected;
			}
		}
		public void Send ( byte[ ] bytes )
		{
			if ( IsOpened )
				try
				{
					client.Client.Send(bytes);
				}
				catch (Exception e)
				{
                    LogHelper.GetLogger<TcpClientPp>().Debug(e.StackTrace);
                    ;
				}

			//if ( IsOpened && ns != null )
			//{
			//    try
			//    {
			//        //client.Client.Send(bytes);
			//        var ins=client.Client.Send(bytes);
			//        if (ins == bytes.Length)
			//        {
			//            ;
			//        }
			//        //ns.BeginWrite(bytes, 0, bytes.Length, write_ok, null);
			//        //ns.Write ( bytes, 0, bytes.Length );
			//    } catch ( Exception e )
			//    {
			//        ;
			//    }
			//}
		}

		//private void write_ok(IAsyncResult ar)
		//{
		//    try
		//    {
		//        //ns.EndWrite(ar);
		//    }
		//    catch (Exception e)
		//    {
		//        ;
		//    }
		//}

		public event EventHandler<SEventArgs<byte[ ]>> DataReceived;
	}
}
