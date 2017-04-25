using System;
using System.Net;
using System.Net.Sockets;
using Summer.System.Log;
using Summer.System.Util;

namespace Summer.System.NET
{
    /// <summary>
    /// 委托函数，应用接收数据后的处理函数，用户在使用时自行定义
    /// </summary>
    public delegate void NetAsyncRxDataCallBack(byte[] data, IPEndPoint remote);

    /// <summary>
    /// UDP监听类，在Config目录下包含net.xml文件，此文件用于指定监听的端口号。
    /// </summary>
    /// 1、构造函数：设置监听的端口号
    /// 2、Open：对端口开启监听，若没有设置端口号，提示错误信息
    /// 3、Receive：同步接收函数，没有接收到数据会一直等待，直到接收到数据才继续往下执行
    /// 4、ReceiveAsync：异步接收函数，没有接收到数据继续往下执行，接收到数据时会去执行接收回调函数
    /// 5、Close：关闭对端口的监听，应用可以调用此函数；若没有调用此函数，系统会自动关闭端口监听
    /// 6、Dispose：释放资源，关闭端口监听；若端口监听已关闭，则无需再关闭
    /// <example>
    /// 1、阻塞方式调用
    /// UdpNetServer server = new UdpNetServer(2000); 
    /// server.Open();
    /// IPEndPoint remote = NetSetAnyIpep.SetAnyIpep();
    /// while(true)
    /// {
    ///     byte[] data = server.Receive(remote);
    /// }
    /// 2、非阻塞方式调用
    /// UdpNetServer server = new UdpNetServer(2000); 
    /// server.Open();
    /// IPEndPoint remote = NetSetAnyIpep.SetAnyIpep();
    /// 
    /// server.ReceiveAsync(remote); //只需要调用一次
    /// 
    /// while(NeedRunning)
    /// {
    ///     Thread.Sleep(100);
    /// }
    /// <note>
    /// 此处非阻塞方式调用有待进一步完善
    /// </note>
    /// </example>
    /// <remark>
    /// 思路：利用UdpClient实现基本的UDP接收数据功能
    /// 公司：CASCO
    /// 作者：田绪俊
    /// 创建日期：2013-5-13
    /// </remark>
    public class UdpNetServer : IDisposable
    {
        private int listeningPort;
        private UdpClient receivingUdpClient;

        public NetAsyncRxDataCallBack AsyncRxProcessCallBack;

        /// <summary>
        /// 构造函数，设置监听的端口号
        /// </summary>
        /// <param name="listenPort"></param>
        /// <returns></returns>
        public UdpNetServer(int listenPort)
        {
            this.listeningPort = listenPort;
        }

        /// <summary>
        /// 对端口开启监听，若监听出错，返回错误信息
        /// </summary>
        /// <returns></returns>
        public void Open()
        {
            try
            {
                receivingUdpClient = new UdpClient(listeningPort);
                LogHelper.GetLogger<UdpNetServer>().Debug(string.Format("Udp Server start listening at:{0}", listeningPort));
            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetServer>().Error(ee.ToString());
                throw ee;
            }
        }

        /// <summary>
        /// 从远程端口同步接收数据
        /// </summary>
        /// <returns></returns>
        public byte[] Receive(IPEndPoint remote)
        {
            try
            {
                byte[] data = receivingUdpClient.Receive(ref remote);

                LogHelper.GetLogger<UdpNetServer>().Debug(string.Format("Received from {0}, Data: {1}",
                    remote.ToString(),
                    ByteHelper.Byte2ReadalbeXstring(data)));

                return data;

            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetServer>().Error(ee.ToString());
                throw ee;      	
            }

        }

        /// <summary>
        /// 从远程端口异步接收数据
        /// <note>
        /// 应用对于接收数据的处理通过定义NetAsyncRxDataCallBack函数来实现
        /// 应用定义的NetAsyncRxDataCallBack函数在接收回调函数ReceiveCallback中完成注册
        /// </note>
        /// </summary>
        /// <returns></returns>
        public void ReceiveAsync(IPEndPoint remote)
        {
            try
            {
                receivingUdpClient.BeginReceive(new AsyncCallback(ReceiveCallback), remote);
            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetServer>().Error(ee.ToString());
                throw ee;
            }

            return;
        }

        ///异步接收的回调函数
        private void ReceiveCallback(IAsyncResult iar)
        {
            try
            {
                IPEndPoint e = iar.AsyncState as IPEndPoint;

                //需要判断当前资源是否已经关闭，如果关闭，则退出接收
                if (iar.IsCompleted && receivingUdpClient.Client!=null)
                {
                    byte[] buffer = receivingUdpClient.EndReceive(iar, ref e);

                    LogHelper.GetLogger<UdpNetServer>().Debug(string.Format("Received from {0}, Data: {1}",
                        e.ToString(),
                        ByteHelper.Byte2ReadalbeXstring(buffer)));

                    if (AsyncRxProcessCallBack != null)
                    {
                        AsyncRxProcessCallBack(buffer, e);
                    }
                }
            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetServer>().Error(ee.ToString());
                throw ee;
            }
            return; 

        }

        /// <summary>
        /// 关闭端口监听
        /// 应用可以调用此函数关闭端口监听；若不调用此函数，系统也会自动关闭监听
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            try
            {
                receivingUdpClient.Close();

                LogHelper.GetLogger<UdpNetServer>().Debug(string.Format("Udp server stop listening at:{0}", listeningPort));

            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetServer>().Error(ee.ToString());
                throw ee;
            }
        }

        /// <summary>
        /// 释放资源，关闭端口监听
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            if (receivingUdpClient != null)
            {
                Close();
            }
        }
    }
}
