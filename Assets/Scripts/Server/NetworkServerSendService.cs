using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace BlueNoah.Net
{

    public class NetworkServerSendService
    {
        int LOCAL_PORT = 50001;
        static IPEndPoint mIPEndPoint;
        string mTargetIP = "127.0.0.1";
        public static UdpClient udp;
        public Thread thread;
        public List<byte[]> datas;
        public Dictionary<string, IPEndPoint> targetPoints;
        const int FrameInterval = 1000;

        public void Init()
        {
            //監視しているポート
            udp = new UdpClient();
            datas = new List<byte[]>();
            targetPoints = new Dictionary<string, IPEndPoint>();
            IPAddress address = IPAddress.Parse(mTargetIP);
            mIPEndPoint = new IPEndPoint(address, LOCAL_PORT);
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
            NetworkServerSendService networkServerSendService = obj as NetworkServerSendService;
            while (isRunning)
            {
                if (networkServerSendService.datas.Count == 0)
                {
                    byte[] data = SerializationUtility.SerializeObject(new BaseMessage());
                    networkServerSendService.datas.Add(data);
                }
                for (int i = 0; i < networkServerSendService.datas.Count; i++)
                {
                    udp.Send(networkServerSendService.datas[i], networkServerSendService.datas[i].Length, mIPEndPoint);
                }
                networkServerSendService.datas.Clear();
                Thread.Sleep(FrameInterval);
            }
            Debug.Log("Thread Done!");
        }

        public void Send<T>(T baseMessage) where T : BaseMessage
        {
            datas.Add(SerializationUtility.SerializeObject(baseMessage));
        }

        public void AddTargetPoint(IPEndPoint iPEndPoint)
        {
            string key = iPEndPoint.Address.ToString() + ":" + iPEndPoint.Port;
            if (!targetPoints.ContainsKey(key))
            {
                targetPoints.Add(key, iPEndPoint);
            }
        }
    }
}
