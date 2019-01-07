using UnityEngine;

namespace BlueNoah.Net
{
    public class NetworkServer : MonoBehaviour
    {
        NetworkServerSendService sendService;

        NetworkServerRecieveService recieveService;

        void Start()
        {
            sendService = new NetworkServerSendService();
            sendService.Init();
            recieveService = new NetworkServerRecieveService();
            recieveService.Init();
        }

		private void Update()
		{
            BaseMessage baseMessage = new BaseMessage();
            baseMessage.id = Time.frameCount;
            //sendService.Send<BaseMessage>(baseMessage);
		}

		private void OnDestroy()
		{
            sendService.StopReceive();
            recieveService.StopReceive();
		}
	}
}
