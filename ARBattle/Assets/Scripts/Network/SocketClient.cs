using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;

public class SocketClient : MonoBehaviour
{

    TcpClient MyClient;
    private Thread ClientThread;
    int MsgIndex;
    Socket ClientSocket;
    IPEndPoint ep;
    string msg;
    byte[] byteSendingArray;
    // Use this for initialization
    void Start()
    {
        ep = new IPEndPoint(IPAddress.Parse("192.168.1.107"), 1001);
        byteSendingArray = new byte[100]; MsgIndex = 0;
        ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendMsg();
        }
    }

    public void SendMsg()
    {
        try
        {
            //Socket Udp
            msg = "客户端：啊~~~" + MsgIndex + "环";
            byteSendingArray = Encoding.Unicode.GetBytes(msg);
            ClientSocket.SendTo(byteSendingArray, ep);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        MsgIndex++;
    }
}
