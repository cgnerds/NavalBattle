using UnityEngine;
using System.Collections;
//网络相关库  
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//发送信息解析库
using System.Text.RegularExpressions;
using System.Collections.Generic;


public class TcpServer : MonoBehaviour {

    Socket serverSocket; //服务器端socket
    Socket clientSocket; //客户端socket
    IPEndPoint ipEnd;    //侦听端口
    public string recvStr; //接收的字符串  
    public string sendStr; //发送的字符串  
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节  
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节  
    int recvLen; //接收的数据长度  
    Thread connectThread; //连接线程 
    public List<int> list = new List<int>(); //触控墙坐标

    //初始化  
    void InitSocket()
    {
        //定义侦听端口,侦听任何IP  
        ipEnd = new IPEndPoint(IPAddress.Any, 5099);
        //定义套接字类型,在主线程中定义  
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //连接  
        serverSocket.Bind(ipEnd);
        //开始侦听,最大10个连接  
        serverSocket.Listen(10);

        //开启一个线程连接，必须的，否则主线程卡死  
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketConnet()
    {
        if (clientSocket != null)
            clientSocket.Close();

        //控制台输出侦听状态  
        print("----Waiting for a client----");
        //一旦接受连接，创建一个客户端  
        clientSocket = serverSocket.Accept();
        //获取客户端的IP和端口  
        IPEndPoint ipEndClient = (IPEndPoint)clientSocket.RemoteEndPoint;
        //输出客户端的IP和端口  
        print("----Connect with " + ipEndClient.Address.ToString() + ":" + ipEndClient.Port.ToString() + "----");
        //连接成功则发送数据  
        sendStr = "----Welcome to the Unity server----";
        SocketSend(sendStr);
    }

    void SocketSend(string sendStr)
    {
        //清空发送缓存  
        sendData = new byte[1024];
        //数据类型转换  
        sendData = Encoding.ASCII.GetBytes(sendStr);
        //发送  
        clientSocket.Send(sendData, sendData.Length, SocketFlags.None);
    }

    void SocketReceive()
    {
        SocketConnet();
        //进入接收循环 
        list.Clear();
        while (true)
        {
            //对data清零  
            recvData = new byte[1024];
            //获取收到的数据的长度  
            recvLen = clientSocket.Receive(recvData);
            //如果收到的数据长度为0，则重连并进入下一个循环  
            if (recvLen == 0)
            {
                SocketConnet();
                continue;
            }
            //输出接收到的数据  
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            foreach (Match m in Regex.Matches(recvStr, @"\d+"))
                list.Add(int.Parse(m.Value));
            SocketSend(sendStr);
            Debug.Log("Reserve12：" );
        }
    }

    void SocketQuit()
    {
        //先关闭客户端
        if (clientSocket != null)
            clientSocket.Close();
        //再关闭线程  
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭服务器  
        serverSocket.Close();
        Debug.Log("----Diconnect----");
    }

    // Use this for initialization
    void Start()
    {
        InitSocket();
    }

    // Update is called once per frame
    void Update()
    {
        Application.runInBackground=true;
    }

    //程序退出则关闭连接  
    void OnApplicationQuit()
    {
        SocketQuit();
    }

};
