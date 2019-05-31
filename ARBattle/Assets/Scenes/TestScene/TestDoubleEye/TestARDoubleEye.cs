using EasyAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARBattleTest
{

    public class TestARDoubleEye : MonoBehaviour
    {
        public RectTransform LeftRawImageRT;    // -580 -480 -380   Range(-200,0 )
        public RectTransform RightRawImageRT; //  580 480 380
        public Slider VerticalSlider;
        public Slider HorizontalSlider;
        public Slider ScaleSlider;
        private Vector2 leftAnchoredPos = new Vector2(-480,0);
        private Vector2 rightAnchoredPos = new Vector2(480, 0);
        void Start()
        {
            HorizontalSlider.value = 0.5f;
            VerticalSlider.value = 0.5f;
            LeftRawImageRT.anchoredPosition = leftAnchoredPos;
            RightRawImageRT.anchoredPosition = rightAnchoredPos;
            Debug.Log(LeftRawImageRT.anchoredPosition);
            Debug.Log(RightRawImageRT.anchoredPosition);
        }

        public void OnHorizontalSliderValueChange()
        {    
            float x = HorizontalSlider.value * 200 - 580; //Range(0, 200) - 580
            LeftRawImageRT.anchoredPosition = new Vector2(x, leftAnchoredPos.y);
            RightRawImageRT.anchoredPosition = new Vector2(-x, rightAnchoredPos.y);

            leftAnchoredPos = LeftRawImageRT.anchoredPosition;
            rightAnchoredPos = RightRawImageRT.anchoredPosition;
        }

        public void OnVerticalSliderValueChange()
        {
            float y = (VerticalSlider.value - 0.5f) * 200; //Range(-100, 100) 
            LeftRawImageRT.anchoredPosition = new Vector2(leftAnchoredPos.x, y );
            RightRawImageRT.anchoredPosition = new Vector2(rightAnchoredPos.x, y);

            leftAnchoredPos = LeftRawImageRT.anchoredPosition;
            rightAnchoredPos = RightRawImageRT.anchoredPosition;
        }

        void Update()
        {
           // Debug.Log(LeftRawImageRT.anchoredPosition);
           // Debug.Log(RightRawImageRT.anchoredPosition);
        }
    }
}
