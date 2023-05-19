using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 引入库
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace sz.network
{
    public class ServSocket : MonoBehaviour
    {
        private Socket serverSocket;           // 服务器socket
        [HideInInspector]
        public Socket clientSocket = null;            // 客户端socket

        private IPEndPoint endpoint;          // 侦听端口

        public string recvStr;         // 接收字符串
        public string sendStr;         // 发送字符串
        private byte[] recvData = new byte[1024];   // 接收数据,必须为字节;
        private byte[] sendData = new byte[1024];   // 发送数据,必须为字节;
        private int recvLen;            // 接收数据长度;

        private Thread connectThread;   // 连接线程
        private Thread dataProcessThread;   // 连接线程

        // serv的ip和端口
        public string _ip = "127.0.0.1";
        public int _port = 10010;

        // 存储客户端信息
        //static Dictionary<string, Socket> clientConnectionItems = new Dictionary<string, Socket> { };
        //private Queue<KeyValuePair<string, string>> quene = new Queue<KeyValuePair<string, string>>();
        private Queue<string> quene = new Queue<string>();

        // Start is called before the first frame update
        void Start()
        {
            InitSocket(); //在这里初始化server
        }

        // Update is called once per frame
        void Update()
        {
            //if (clientSocket == null)
            //    Debug.Log(Time.timeSinceLevelLoad + ":clientSocket is null!");
        }

        void InitSocket()
        {

            // 创建对象端口,侦听任意ip
            try
            {
                endpoint = new IPEndPoint(IPAddress.Any, _port);            //定义端口
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //定义套接字
                serverSocket.Bind(endpoint);                                //连接
                serverSocket.Listen(10);                                    //最大监听队列长度

                // 开启一个线程连接, 否则主线程会卡死                                
                connectThread = new Thread(new ThreadStart(SocketConnet));
                connectThread.Start();

            }
            catch (Exception e)
            {
                // Console.WriteLine(e.Message);
                Debug.Log(e.Message);
            }
        }

        //连接
        void SocketConnet()
        {
            while (true)
            {
                // 监听
                Socket localSocket = serverSocket.Accept();

                //// 有新连接会挤掉旧连接；
                //if (clientSocket != null && clientSocket.Connected) {
                //    clientSocket.Close();
                //    clientSocket = null;
                //}
                clientSocket = localSocket;
                //获取客户端的IP和端口
                IPEndPoint ipEndClient = (IPEndPoint)localSocket.RemoteEndPoint;

                //////clientConnectionItems.Add(ipEndClient.ToString(), localSocket);

                //输出客户端的IP和端口
                Debug.Log("Connect with " + ipEndClient.Address.ToString() + ":" + ipEndClient.Port.ToString());
                //连接成功则发送数据
                string sendStr = "Welcome!, This is Unity service";
                //SocketSend(sendStr, ipEndClient.ToString());
                SocketSend(sendStr);

                // 开启一个线程连接, 否则主线程会卡死                                
                dataProcessThread = new Thread(new ThreadStart(SocketReceive));
                dataProcessThread.Start();
                //dataProcessThread = new Thread(new ParameterizedThreadStart(SocketReceive));
                //dataProcessThread.Start(localSocket);
            }
        }

        //public void SocketSend(string sendStr, string ipClient)
        public void SocketSend(string sendStr)
        {
            //清空发送缓存
            sendData = new byte[1024];
            //数据类型转换
            sendData = Encoding.UTF8.GetBytes(sendStr);    // send buffer
                                                           // sendData = Encoding.UTF8.GetBytes(sendStr);

            //发送
            //clientConnectionItems[ipClient].Send(sendData, sendData.Length, SocketFlags.None);
            clientSocket.Send(sendData, sendData.Length, SocketFlags.None);
        }

        //服务器接收
        //public void SocketReceive(object socketclientpara)
        public void SocketReceive()
        {
            //Socket clientSocket = socketclientpara as Socket;

            //进入接收循环
            while (true)
            {
                //清空缓存
                recvData = new byte[1024];
                //获取收到的数据的长度
                try
                {
                    // 客户端断开
                    if (clientSocket == null || !clientSocket.Connected)
                    {
                        // continue;

                        break;
                    }

                    recvLen = clientSocket.Receive(recvData);
                    //如果收到的数据长度为0，则重连并进入下一个循环
                    if (recvLen == 0)
                    {
                        //SocketConnet();
                        //continue;
                        // clientConnectionItems.Remove(clientSocket.RemoteEndPoint.ToString());
                        break;
                    }
                    //输出接收到的数据
                    // recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
                    //recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
                    string recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
                    Debug.Log(recvStr);

                    // recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
                    // Debug.Log(recvStr);
                    //this.quene.Enqueue(new KeyValuePair<string, string>(clientSocket.RemoteEndPoint.ToString(), recvStr));
                    this.quene.Enqueue(recvStr);
                    //将接收到的数据经过处理再发送出去
                    //sendStr = "From Server: " + recvStr;
                    //SocketSend(sendStr);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    //clientConnectionItems[clientSocket.RemoteEndPoint.ToString()].Close();
                    //clientSocket.Close();
                }
            }

        }

        //连接关闭
        void SocketQuit()
        {
            //先关闭客户端 // 遍历字典关闭
            //foreach(KeyValuePair<string, Socket> elem in clientConnectionItems)
            //if (elem.Value != null)
            //    elem.Value.Close();

            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
               
                
            //再关闭线程
            if (connectThread != null)
            {
                connectThread.Interrupt();
                connectThread.Abort();
            }
            //最后关闭服务器
            serverSocket.Close();
            print("diconnect");
        }

        //public KeyValuePair<string, string> GetRecvStr()
        public string GetRecvStr()
        {
            string returnStr;
            // KeyValuePair<string, string> returnStr ;
            // 加线程锁，禁止字符写操作
            lock (this)
            {
                // 队列内有数据时，取数据
                if (quene.Count > 0)
                {
                    returnStr = quene.Dequeue();
                    return returnStr;
                }
            }
            //return new KeyValuePair<string, string>();          //无法返回空null
            return null;
        }


        void OnApplicationQuit()
        {
            SocketQuit();
        }

    }

}
