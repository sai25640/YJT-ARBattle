using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARBattleTest
{

    public class TestDoubleEye : MonoBehaviour
    {
        public Text EyeTxt;
        private EyeMode mEyeMode;
        public GameObject DoubleEye;
        public GameObject SingeEye;
        //public Canvas Canvas;
        //public Camera DoubleEyeUICamera;
        //public Camera SingleEyeUICamera;
        void Start()
        {
            mEyeMode = EyeMode.Single;
            //SingeEye.gameObject.SetActive(true);
            DoubleEye.gameObject.SetActive(true);
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    mEyeMode = EyeMode.Double;
            //}
            //if (Input.GetKeyUp(KeyCode.D))
            //{
            //    mEyeMode = EyeMode.Single;
            //}

            //UpdateEyeMode();
        }

        void UpdateEyeMode()
        {
            if (mEyeMode == EyeMode.Double)
            {
                SingeEye.gameObject.SetActive(false);
                DoubleEye.gameObject.SetActive(true);
             
                EyeTxt.text = "DoubleEye";
            }
            else
            {
                SingeEye.gameObject.SetActive(true);
                DoubleEye.gameObject.SetActive(false);

                EyeTxt.text = "SingeEye";
            }
        }
    }
}
