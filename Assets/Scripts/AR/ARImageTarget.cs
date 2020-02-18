using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using EasyAR;
namespace ARBattle
{
    public class ARImageTarget : MonoBehaviour
    {
        private ImageTargetBehaviour mImageTargetBehaviour;
        void Start()
        {
            Log.I("ARImageTarget Active = true");
//#if UNITY_EDITOR
            GameManager.Instance.StartGame();
//#endif
            mImageTargetBehaviour = GetComponent<ImageTargetBehaviour>();

            mImageTargetBehaviour.TargetFound += TargetFound;

            mImageTargetBehaviour.TargetLost += TargetLost;
        }

        private void TargetFound(TargetAbstractBehaviour targetAbstractBehaviour)
        {
            //GameManager.Instance.ShowScene();
            EventCenter.Broadcast(EventDefine.TargetFound);
            Debug.Log("TargetFound");
        }

        private void TargetLost(TargetAbstractBehaviour targetAbstractBehaviour)
        {
            //GameManager.Instance.HideScene();
            EventCenter.Broadcast(EventDefine.TargetLost);
        }
    }
}
