using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using ARBattle;
using UnityEngine.Experimental.PlayerLoop;


public class SocketServer : MonoBehaviour
{
    private Thread _StartServer;

    byte[] byte_receive;
    Socket RecieveSocket;
    IPEndPoint iep;
    EndPoint ep;
     bool StartGame = false;
    // Use this for initialization
    void Start()
    {

        //Socket Udp
        byte_receive = new byte[100];
        iep = new IPEndPoint(IPAddress.Any, 9988);
        RecieveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        RecieveSocket.Bind(iep);
        ep = (EndPoint)iep;

        _StartServer = new Thread(StartServer);
        _StartServer.Start();

    }

    void StartServer()
    {
        //Socket Udp
        while (true)
        {
            int intReceiveLength = RecieveSocket.ReceiveFrom(byte_receive, ref ep);
            string strReceiveStr = Encoding.Unicode.GetString(byte_receive, 0, intReceiveLength);
            Debug.Log("收到消息：  " + strReceiveStr);
            if (strReceiveStr.Equals("Attack"))
            {
                EventCenter.Broadcast(EventDefine.AttackFire);
            }
            else if(strReceiveStr.Equals("StartGame"))
            {
                StartGame = true;          
            }
        }
    }

    void Update()
    {
        if (StartGame)
        {
            GameManager.Instance.StartGame();
            StartGame = false;
        }
    }


    private void OnApplicationQuit()
    {
        if (_StartServer != null)
        {
            _StartServer.Interrupt();
            _StartServer.Abort();
        }
        //最后关闭socket
        if (RecieveSocket != null)
            RecieveSocket.Close();
    }
}
