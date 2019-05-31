using EasyAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace ARBattle
{
    /// <summary>
    /// 管理AR相关的
    /// </summary>
    public class ARManager : MonoSingleton<ARManager>
    {
        ResLoader mResLoader = ResLoader.Allocate();
        GameObject mEasyARObj;
        ImageTrackerBehaviour mImageTracker;
        public Transform ARCamera;
        public bool IsTargetFound = false;
        void Awake()
        {
            EventCenter.AddListener(EventDefine.TargetFound, TargetFound);
            EventCenter.AddListener(EventDefine.TargetLost, TargetLost);
        }

        public void Init()
        {
            //EasyAR
            //mEasyARObj = mResLoader.LoadSync<GameObject>("EasyAR_Startup").Instantiate();

            //ARCamer
            //ARCamera = mEasyARObj.transform.Find("ARCamera");
            ARCamera = GameObject.Find("ARCamera").transform;
            //ImageTracker
            //mImageTracker = mEasyARObj.transform.Find("ImageTracker").transform.GetComponent<ImageTrackerBehaviour>();

            // var target = mResLoader.LoadSync<GameObject>("ImageTarget").Instantiate();
            // target.transform.GetComponent<ImageTargetBehaviour>().Bind(mImageTracker);
        }

        private void TargetFound()
        {
            IsTargetFound = true;
        }

        private void TargetLost()
        {
            IsTargetFound = false;
        }

        protected override void OnDestroy()
        {
            EventCenter.RemoveListener(EventDefine.TargetFound, TargetFound);
            EventCenter.RemoveListener(EventDefine.TargetLost, TargetLost);

            mEasyARObj.DestroySelfGracefully();
            mImageTracker.DestroySelfGracefully();
            ARCamera.DestroySelfGracefully();
            mResLoader.Recycle2Cache();
            mResLoader = null;
            base.OnDestroy();

        }
    }
}
