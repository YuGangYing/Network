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

        public UnityAction<byte[],IPEndPoint> onRecieve;

        public void Init()
        {
            //監視しているポート
            udp = new UdpClient(NetworkConstant.SERVER_PORT);
            thread = new Thread(ThreadMethod);
            thread.IsBackground = true;
            thread.Start(this);
        }

        public void OnRecieve(byte[] data,IPEndPoint iPEndPoint)
        {
            Debug.Log(System.DateTime.Now.Ticks / 10000);
            if (onRecieve != null)
                onRecieve(data,iPEndPoint);
        }

        public void StopReceive()
        {
            udp.Close();
            thread.Abort();
        }

        public bool isRunning = true;
        static void ThreadMethod(object obj)
        {
            NetworkServerRecieveService networkServerRecieveService = obj as NetworkServerRecieveService;
            while (networkServerRecieveService.isRunning)
            {
                //メセージを受け取っていない時、読み取ない。
                if (networkServerRecieveService.udp.Available == 0)
                {
                    continue;
                }
                IPEndPoint remoteEP = null;
                byte[] data = networkServerRecieveService.udp.Receive(ref remoteEP);
                //BaseMessage baseMessage = SerializationUtility.DeserializeObject(data) as BaseMessage;
                if(networkServerRecieveService!=null)
                    networkServerRecieveService.OnRecieve(data,remoteEP);
            }
            Debug.Log("Thread Done!");
        }
    }
}