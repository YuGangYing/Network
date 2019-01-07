using UnityEngine;
using BlueNoah.Net;

public class ActionController : MonoBehaviour {

    NetworkClient mNetworkClient;

	void Awake()
	{
        mNetworkClient = GetComponent<NetworkClient>();
	}

	void Start () {
        HandleMessage handleMessage = new HandleMessage();
        handleMessage.handleId = 99;
        byte[] data = SerializationUtility.SerializeObject((BaseMessage)handleMessage);
        object obj = SerializationUtility.DeserializeObject(data);
        handleMessage = (HandleMessage)obj;
        Debug.Log(handleMessage.handleId);

        mNetworkClient.Register(FunctionConstant.CHANGE_DIRECT,OnChangeDirect);
	}
	
    void OnChangeDirect(BaseMessage baseMessage){
        HandleMessage handleMessage = baseMessage as HandleMessage;
        Debug.Log(handleMessage.handleId);
    }

	void Update () {
        if(Input.GetKeyDown(KeyCode.A))
        {
            HandleMessage handleMessage = new HandleMessage();
            handleMessage.handleId = 1;
            mNetworkClient.Send(FunctionConstant.CHANGE_DIRECT,handleMessage);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
        if (Input.GetKeyDown(KeyCode.D))
        {

        }
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
	}
}
