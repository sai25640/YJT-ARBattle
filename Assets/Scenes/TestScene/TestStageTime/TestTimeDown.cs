using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARBattleTest
{

    public class TestTimeDown : MonoBehaviour
    {
        private Animator mAnimator;

        void Start()
        {
            mAnimator = GetComponent<Animator>();
            mAnimator.SetBool("CountDown", false);
        }

        void CountDown()
        {
            mAnimator.SetBool("CountDown", true);
        }

        public void TimeDownEvent()
        {
            Debug.Log("TimeDownEvent");
        }

        public void GameStartEvent()
        {
            Debug.Log("GameStartEvent");
        }

        public void AnimationOnEndEvent()
        {
            Debug.Log("AnimationOnEndEvent");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                CountDown();
            }
        }
    }
}
