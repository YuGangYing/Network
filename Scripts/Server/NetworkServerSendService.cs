using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;

namespace BlueNoah.Net
{
    public class NetworkServerSendService
    {
        //static IPEndPoint mIPEndPoint;
        public static UdpClient udp;
        public Thread thread;
        //public List<BaseMessage> messages;
        public List<byte[]> datas;
        public Dictionary<string, IPEndPoint> targetPoints;
        public bool isBegin = false;

        public void Init()
        {
            //監視しているポート
            udp = new UdpClient();
            datas = new List<byte[]>();
            targetPoints = new Dictionary<string, IPEndPoint>();
            //IPAddress address = IPAddress.Parse(NetworkConstant.CLIENT_IP);
            //mIPEndPoint = new IPEndPoint(address, NetworkConstant.CLIENT_PORT);
            thread = new Thread(ThreadMethod);
            thread.IsBackground = true;
            thread.Start(this);
        }

        public void StopReceive()
        {
            udp.Close();
            thread.Abort();

        }

        public bool isRunning = true;
        static void ThreadMethod(object obj)
        {
            NetworkServerSendService networkServerSendService = obj as NetworkServerSendService;
            long startTime = DateTime.Now.Ticks / 10000;
            long frame = 0;
            while (networkServerSendService.isRunning)
            {
                if (networkServerSendService.isBegin)
                {
                    if (networkServerSendService.datas.Count == 0)
                    {
                        BaseMessage baseMessage = new BaseMessage();
                        baseMessage.frame = frame;
                        networkServerSendService.datas.Add(SerializationUtility.SerializeObject(baseMessage));
                    }

                    foreach (IPEndPoint iPEndPoint in networkServerSendService.targetPoints.Values)
                    {
                        for (int i = 0; i < networkServerSendService.datas.Count; i++)
                        {
                            udp.Send(networkServerSendService.datas[i], networkServerSendService.datas[i].Length, iPEndPoint);
                        }
                    }

                    networkServerSendService.datas.Clear();
                    long nowTime = DateTime.Now.Ticks / 10000;
                    long time = startTime + frame * NetworkConstant.SERVER_SEND_RATE;
                    int offset = (int)(nowTime - time);
                    Thread.Sleep(Mathf.Max(0, NetworkConstant.SERVER_SEND_RATE - offset));
                    frame++;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            Debug.Log("Thread Done!");
        }

        public void SendToAll(byte[] data)
        {
            Debug.Log("Send");
            datas.Add(data);
        }
    }
}
