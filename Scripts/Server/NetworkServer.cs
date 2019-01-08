using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace BlueNoah.Net
{
    public class NetworkServer : MonoBehaviour
    {
        NetworkServerSendService mNetworkServerSendService;

        NetworkServerRecieveService mNetworkServerRecieveService;

        Dictionary<string, IPEndPoint> mTargetPoints;

        int mMaxPlayer = 1;

        bool mIsBegin = false;

        void Start()
        {
            mTargetPoints = new Dictionary<string, IPEndPoint>();
            mNetworkServerSendService = new NetworkServerSendService();
            mNetworkServerSendService.Init();
            mNetworkServerSendService.targetPoints = mTargetPoints;
            mNetworkServerRecieveService = new NetworkServerRecieveService();
            mNetworkServerRecieveService.Init();
            mNetworkServerRecieveService.onRecieve = OnRecieve;
        }

        void OnDestroy()
        {
            mNetworkServerSendService.StopReceive();
            mNetworkServerRecieveService.StopReceive();
        }

        public void OnRecieve(byte[] data, IPEndPoint iPEndPoint)
        {
            if(!mIsBegin){
                if (!mTargetPoints.ContainsKey(iPEndPoint.ToString()))
                {
                    mTargetPoints.Add(iPEndPoint.ToString(), iPEndPoint);
                    Debug.Log("<color=green> Player Enter. </color>");
                    if (mTargetPoints.Count == Mathf.Max(1,mMaxPlayer) )
                    {
                        mIsBegin = true;
                        mNetworkServerSendService.isBegin = true;
                        Debug.Log("<color=yellow> Begin. </color>");
                    }
                }
            }else{
                mNetworkServerSendService.SendToAll(data);
            }
           
        }
    }
}
