using System.Collections;
using System.Collections.Generic;
using EasyAR;
using UnityEngine;
using UnityEngine.UI;

namespace ARBattleTest
{
    enum EyeMode
    {
        Single,
        Double
    }

    public class TestSimpleDoubleEye : MonoBehaviour
    {
        public Text EyeTxt;
        private EyeMode mEyeMode;
        public GameObject DoubleEye;
        public GameObject SingeEye;

        void Start()
        {
            mEyeMode = EyeMode.Single;
            DoubleEye.gameObject.SetActive(false);
            SingeEye.gameObject.SetActive(true);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                mEyeMode = EyeMode.Double;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                mEyeMode = EyeMode.Single;
            }

            UpdateEyeMode();
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
