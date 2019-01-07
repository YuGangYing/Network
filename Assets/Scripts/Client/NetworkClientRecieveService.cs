using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using UnityEngine.Events;
using System.Collections.Generic;

namespace BlueNoah.Net
{
    public class NetworkClientRecieveService
    {
        //メセージを監視用UDPクライアント。
        public UdpClient udp;
        //メセージを監視用スレッド
        public Thread thread;

        public UnityAction<NetworkData> onRecieve;

        public Dictionary<short, UnityAction<BaseMessage>> functionDic;

        public void Init()
        {
            //監視しているポート
            functionDic = new Dictionary<short, UnityAction<BaseMessage>>();
            udp = new UdpClient(NetworkConstant.CLIENT_PORT);
            thread = new Thread(ThreadMethod);
            thread.IsBackground = true;
            thread.Start(this);
        }

        public void OnRecieve(NetworkData networkData)
        {
            Debug.Log(System.DateTime.Now.Ticks / 10000);
            if (functionDic.ContainsKey(networkData.functionId))
            {
                functionDic[networkData.functionId](networkData.baseMessage);
            }
            //if (onRecieve != null)
                //onRecieve(networkData);
        }

        public void StopReceive()
        {
            udp.Close();
            thread.Abort();
        }

        public void Register(short functionId, UnityAction<BaseMessage> action)
        {
            if (!functionDic.ContainsKey(functionId))
            {
                functionDic.Add(functionId, action);
            }
        }

        public bool isRunning = true;
        static void ThreadMethod(object obj)
        {
            NetworkClientRecieveService networkClientRecieveService = obj as NetworkClientRecieveService;
            Debug.Log("NetworkClientRecieveService Prepared.");
            while (networkClientRecieveService.isRunning)
            {
                //メセージを受け取っていない時、読み取ない。
                if (networkClientRecieveService.udp.Available == 0)
                {
                    continue;
                }
                IPEndPoint remoteEP = null;
                byte[] data = networkClientRecieveService.udp.Receive(ref remoteEP);
                NetworkData networkData = SerializationUtility.DeserializeObject(data) as NetworkData;
                networkClientRecieveService.OnRecieve(networkData);
            }
            Debug.Log("Thread Done!");
        }
    }
}