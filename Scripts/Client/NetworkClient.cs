using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.Net
{
    public class NetworkClient : MonoBehaviour
    {

        NetworkClientSendService mNetworkClientSendService;

        NetworkClientRecieveService mNetworkClientRecieveService;

        void Awake()
        {
            mNetworkClientSendService = new NetworkClientSendService();
            mNetworkClientSendService.Init();
            mNetworkClientRecieveService = new NetworkClientRecieveService();
            mNetworkClientRecieveService.Init();
        }

        void Update()
        {
            BaseMessage baseMessage = new BaseMessage();
            baseMessage.id = Time.frameCount;
        }

        void OnDestroy()
        {
            mNetworkClientSendService.StopReceive();
            mNetworkClientRecieveService.StopReceive();
        }

        public void Send(short functionId, BaseMessage baseMessage)
        {
            NetworkData networkData = new NetworkData();
            networkData.functionId = functionId;
            networkData.baseMessage = baseMessage;
            mNetworkClientSendService.Send(networkData);
        }

        public void Register(short functionId, UnityAction<BaseMessage> action)
        {
            mNetworkClientRecieveService.Register(functionId, action);
        }
    }
}
