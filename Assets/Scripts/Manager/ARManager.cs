using EasyAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.PostProcessing;

namespace ARBattle
{
    public enum EyeMode
    {
        SingleEye,
        DoubleEye
    }
    /// <summary>
    /// 管理AR相关的
    /// </summary>
    public class ARManager : MonoSingleton<ARManager>
    {
        ResLoader mResLoader = ResLoader.Allocate();
        GameObject mEasyARObj;
        ImageTrackerBehaviour mImageTracker;
        private GameObject mDoubleEye;
        //private GameObject mUIRenderCamera;
        private GameObject mUICamera;
        private CameraDeviceBehaviour mCameraDevice;
        public Transform ARCamera;
        public bool IsTargetFound = false;
        void Awake()
        {
            EventCenter.AddListener(EventDefine.TargetFound, TargetFound);
            EventCenter.AddListener(EventDefine.TargetLost, TargetLost);
            Init();
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
            mCameraDevice = GameObject.Find("CameraDevice").GetComponent<CameraDeviceBehaviour>();
            mDoubleEye = GameObject.Find("DoubleEye");
           // mUIRenderCamera = GameObject.Find("UIRenderCamera");
            mUICamera = GameObject.Find("UICamera");
        }

        private void Start()
        {
#if UNITY_EDITOR
            SelectMode(EyeMode.SingleEye);
#else
            SelectMode(EyeMode.DoubleEye);
#endif
        }

        private void SelectMode(EyeMode mode)
        {
            switch (mode)
            {
                case EyeMode.SingleEye:
                    {
                        mDoubleEye.SetActive(false);
                        var RenderCamera = ARCamera.transform.Find("RenderCamera");
                        RenderCamera.GetComponent<Camera>().cullingMask = -1;
                        RenderCamera.GetComponent<PostProcessingBehaviour>().enabled = true;
                        RenderCamera.GetComponent<ScreenBlood>().enabled = true;
                        var Camera = ARCamera.transform.Find("RenderCamera/RealityPlane/Camera");
                        Camera.gameObject.SetActive(false);
                        mUICamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
                        mUICamera.GetComponent<Camera>().targetTexture = null;
                    }

                    break;
                case EyeMode.DoubleEye:
                    {
                        mDoubleEye.SetActive(true);
                        var RenderCamera = ARCamera.transform.Find("RenderCamera");
                        RenderCamera.GetComponent<Camera>().cullingMask = 0;
                        RenderCamera.GetComponent<PostProcessingBehaviour>().enabled = false;
                        RenderCamera.GetComponent<ScreenBlood>().enabled = false;
                        var Camera = ARCamera.transform.Find("RenderCamera/RealityPlane/Camera");
                        Camera.gameObject.SetActive(true);
                        mUICamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
                        mCameraDevice.CameraDeviceType = CameraDeviceBaseBehaviour.DeviceType.Default;
                        mCameraDevice.OpenAndStart();
                    }
                    break;
                default:
                    break;
            }
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
