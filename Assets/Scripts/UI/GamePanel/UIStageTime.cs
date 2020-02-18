/****************************************************************************
 * 2019.6 DESKTOP-D8NOAKS
 ****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UniRx;

namespace ARBattle
{
	public partial class UIStageTime : UIElement
	{
        private Text minutesText;
        private Text secondsText;
        private float stageTime = 0;
        private float timer = 0;
        private IntReactiveProperty Minutes = new IntReactiveProperty(0);
        private IntReactiveProperty Seconds = new IntReactiveProperty(0);
        void Awake()
        {
            minutesText = transform.GetChild(0).GetComponent<Text>();
            secondsText = transform.GetChild(1).GetComponent<Text>();
            Minutes.SubscribeToText(minutesText);
            Seconds.SubscribeToText(secondsText);
            HideText();
        }

        public void UpdateStageTime(float time)
        {
            StopCoroutine("Timer");

            stageTime = time;

            ShowText();

            StartCoroutine("Timer");
            //Observable.EveryUpdate().Subscribe(_ =>
            //{          
            //    timer += Time.deltaTime;
            //    if (timer>=1)
            //    {
            //        timer = 0;
            //        stageTime -= 1;

            //        if (stageTime <= 0)
            //        {
            //            stageTime = 0;
            //            HideText();
            //        }

            //        SetMinutesAndSeconds(stageTime);
            //    }

            //}).AddTo(this);
        }

	    IEnumerator Timer()
	    {
	        while (true)
	        {
	            yield return new WaitForSeconds(1f);
                stageTime -= 1;

                if (stageTime <= 0)
                {
                    stageTime = 0;
                    HideText();
                }

                SetMinutesAndSeconds(stageTime);
            }
	    }

        void SetMinutesAndSeconds(float time)
        {
            Minutes.Value = (int)time / 60;
            Seconds.Value = (int)time % 60;
        }

        void ShowText()
        {
            minutesText.transform.parent.gameObject.SetActive(true);
            SetMinutesAndSeconds(stageTime);
        }

        void HideText()
        {
            minutesText.transform.parent.gameObject.SetActive(false);
        }

        protected override void OnBeforeDestroy()
        {
        }
    }
}