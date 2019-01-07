using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace BlueNoah.Net
{

    public class NetworkClientSendService
    {
        static IPEndPoint mIPEndPoint;
        public static UdpClient udp;
        public Thread thread;
        public List<NetworkData> networkDatas;

        public void Init()
        {
            //監視しているポート
            networkDatas = new List<NetworkData>();
            udp = new UdpClient();
            IPAddress address = IPAddress.Parse(NetworkConstant.SERVER_IP);
            mIPEndPoint = new IPEndPoint(address, NetworkConstant.SERVER_PORT);
            thread = new Thread(ThreadMethod);
            thread.IsBackground = true;
            thread.Start(this);
        }

        public void StopReceive()
        {
            udp.Close();
            thread.Abort();
        }

        static bool isRunning = true;
        static void ThreadMethod(object obj)
        {
            NetworkClientSendService networkClientSendService = obj as NetworkClientSendService;
            while (isRunning)
            {
                if (networkClientSendService.networkDatas.Count > 0)
                {
                    for (int i = 0; i < networkClientSendService.networkDatas.Count; i++)
                    {
                        byte[] data = SerializationUtility.SerializeObject(networkClientSendService.networkDatas[i]);
                        udp.Send(data, data.Length, mIPEndPoint);
                    }
                    networkClientSendService.networkDatas.Clear();
                }
                Thread.Sleep(NetworkConstant.CLIENT_SEND_RATE);
            }
            Debug.Log("Thread Done!");
        }

        public void Send(NetworkData networkData)
        {
            networkDatas.Add(networkData);
        }

    }
}
