using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Summer.System.Log;
using Summer.System.Util;

namespace Summer.System.NET
{
    /// <summary>
    /// UDP发送类，用于向远程端口发送数据,在Config目录下包含net.xml文件，此文件用于指定远程的IP地址和端口号。
    /// 1、构造函数：设置远端IP地址和端口号
    /// 2、Connect：连接远程端口，若没有设置远程IP地址与端口号，会提示出错信息
    /// 3、Send：发送函数，应用无需自行建立连接，发送函数会判断是否已经建立连接；若没有，系统调用Connect函数自动建立连接
    /// 4、Close：关闭与远程端口的连接，应用可以调用此函数；若不调用，系统也会自行关闭连接
    /// 5、Dispose：应用无需自行关闭连接，系统会调用Close函数自动关闭连接；若连接已关闭，则不再调用Close函数
    /// </summary>
    /// <remark>
    /// 思路：利用UdpClient类实现基本的UDP发送数据功能
    /// 公司：CASCO
    /// 作者：田绪俊
    /// 创建日期：2013-5-13
    /// </remark>
    public class UdpNetClient : IDisposable
    {
        private IPEndPoint remote;
        private bool isConnected = false;
        private UdpClient udpClient;

        /// <summary>
        /// 构造函数，设置远端IP地址和端口号
        /// </summary>
        /// <param name="remote"></param>
        /// <returns></returns>
        public UdpNetClient(IPEndPoint remote)
        {
            this.remote = remote;
        }

        /// <summary>
        /// 构造函数，设置远程IP地址和端口号
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public UdpNetClient(string ip, int port)
        {
            this.remote = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        /// 连接远程端口
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            try
            {
                udpClient = new UdpClient();
                udpClient.Connect(remote);
                isConnected = true;
                LogHelper.GetLogger<UdpNetClient>().Debug(string.Format("Udp Client has connected to {0}",remote.ToString()));
            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetClient>().Error(ee.ToString());
                throw ee;
            }
        }

        /// <summary>
        /// 发送函数，将数据发送至远程端口，如果没有建立连接，系统自动建立
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void Send(byte[] data)
        {
            if (!isConnected)
            {
                Connect();
            }
            try
            {
                udpClient.Send(data, data.Length);

                LogHelper.GetLogger<UdpNetClient>().Debug(string.Format("Udp Client sent to {0}, Data: {1}",
                    remote.ToString(),
                    ByteHelper.Byte2ReadalbeXstring(data)));
            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetClient>().Error(ee.ToString());
                throw ee;
            }
        }

        /// <summary>
        /// 关闭与远程端口的连接
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            try
            {
                udpClient.Close();
                udpClient = null;
                LogHelper.GetLogger<UdpNetClient>().Debug(string.Format("Udp Client closed the connection to {0}", remote.ToString()));
            }
            catch (Exception ee)
            {
                LogHelper.GetLogger<UdpNetClient>().Error(ee.ToString());
                throw ee;
            }
        }

        /// <summary>
        /// 释放资源，关闭连接
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            if (udpClient != null)
            {
                Close();
            }            
        }
    }
}
