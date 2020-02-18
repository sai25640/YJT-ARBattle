using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
namespace ARBattleTest
{
    public class TestStageTime : MonoBehaviour
    {
        private Text minutesText;
        private Text secondsText;
        private float stageTime = 0;
        private float timer = 0;
        private IntReactiveProperty Minutes = new IntReactiveProperty(0);
        private IntReactiveProperty Seconds = new IntReactiveProperty(0);
        void Start()
        {
            minutesText = transform.GetChild(0).GetComponent<Text>();
            secondsText = transform.GetChild(1).GetComponent<Text>();
            Minutes.SubscribeToText(minutesText);
            Seconds.SubscribeToText(secondsText);

            HideText();
            SetStageTime(70);
        }

        public void  SetStageTime(float time)
        {
            stageTime = time;

            ShowText();

            Observable.EveryUpdate().Subscribe(_ =>
            {
                SetMinutesAndSeconds(stageTime);
                stageTime -= Time.deltaTime;
                if (stageTime<=0)
                {
                    stageTime = 0;
                    HideText();
                }
            }).AddTo(this);
        }

        void SetMinutesAndSeconds(float time)
        {
            Minutes.Value = (int)time / 60;
            Seconds.Value = (int) time % 60;
        }

        void ShowText()
        {
            minutesText.transform.parent.gameObject.SetActive(true);
        }

        void HideText()
        {
            minutesText.transform.parent.gameObject.SetActive(false);
        }
    }
}
