using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using UnityEngine.Events;

namespace BlueNoah.Net
{
    public class NetworkServerRecieveService
    {
        //メセージを監視用UDPクライアント。
        public UdpClient udp;
        //メセージを監視用スレッド
        public Thread thread;

        public UnityAction<BaseMessage> onRecieve;

        public void Init()
        {
            //監視しているポート
            int LOCAL_PORT = 50001;
            udp = new UdpClient(LOCAL_PORT);
            thread = new Thread(ThreadMethod);
            thread.IsBackground = true;
            thread.Start(this);
            onRecieve = OnRecieve;
        }

        public void OnRecieve(BaseMessage baseMessage)
        {
            Debug.Log(baseMessage.id);
        }

        public void StopReceive()
        {
            udp.Close();
            thread.Abort();
        }

        static bool isRunning = true;
        static void ThreadMethod(object obj)
        {
            NetworkServerRecieveService networkServerRecieveService = obj as NetworkServerRecieveService;
            while (isRunning)
            {
                //メセージを受け取っていない時、読み取ない。
                if (networkServerRecieveService.udp.Available == 0)
                {
                    //Thread.Sleep (100);
                    continue;
                }
                IPEndPoint remoteEP = null;
                byte[] data = networkServerRecieveService.udp.Receive(ref remoteEP);
                BaseMessage baseMessage = SerializationUtility.DeserializeObject(data) as BaseMessage;
                networkServerRecieveService.OnRecieve(baseMessage);
            }
            Debug.Log("Thread Done!");
        }
    }
}