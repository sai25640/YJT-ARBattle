/****************************************************************************
 * 2019.6 DESKTOP-D8NOAKS
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ARBattle
{
	public partial class UITimeDown : UIElement
	{
	    private Animator mAnimator;

        private void Awake()
		{
            mAnimator = GetComponent<Animator>();
        }

        public void CountDown()
        {
            Debug.Log("SetTrigger CountDown");

            mAnimator.SetTrigger("CountDown");
        }

        public void TimeDownEvent()
        {
            Debug.Log("TimeDownEvent");

            AudioManager.PlayVoice("TimeDown");
            AudioManager.Instance.SetMusicVolume(0.2f);
        }

        public void GameStartEvent()
        {
            Debug.Log("GameStartEvent");

            StageSystem.Instance.Play();

            AudioManager.PlayMusic("05.GluttonyBattle",volume:0.5f);

            AudioManager.Instance.SetMusicVolume(0.5f);

            EventCenter.Broadcast<float>(EventDefine.UpdateStageTime, StageSystem.Instance.GetStageTime());
        }

        public void AnimationOnEndEvent()
        {
            Debug.Log("AnimationOnEndEvent");         
        }

        protected override void OnBeforeDestroy()
		{
		}
	}
}