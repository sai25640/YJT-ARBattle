using System;
using QFramework;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ARBattle
{
    [RequireComponent(typeof(AnimationModel))]
    public class GuideEnemy :BaseCharacter
    {
        protected Animator mAnim;
        protected NavMeshAgent mAgent;
        protected BaseCharacter mTarget;
        protected AnimationModel mAniModel;
        
        public override void Awake()
        {
            base.Awake();
     
            mAnim = GetComponent<Animator>();
            mAgent = GetComponent<NavMeshAgent>();
            mAniModel = GetComponent<AnimationModel>();
            mTarget = GameManager.Instance.Player.GetComponent<GamePlayer>();
     
        }

        public override void Attack(BaseCharacter target)
        {
      
        }

        public override void Killed()
        {
           
        }

        public override void Move(BaseCharacter target)
        {
      
        }

        public override void UnderAttack(BaseCharacter target)
        {

        }

        public void UnderAttack()
        {
            PlayAnim(mAniModel.HitCount, "Hit", "HitValue");
            mAgent.isStopped = true;
      
            AudioManager.PlaySound("zombie_attack_1");

            StageSystem.Instance.NextStep();
        }

        private void PlayAnim(int clipCount, string triggerName, string floatName)
        {
            float rate = 1.0f / (clipCount - 1);
            //mAnim.CrossFade("Run");
            mAnim.SetTrigger(triggerName);
            mAnim.SetFloat(floatName, rate * Random.Range(0, clipCount));
        }


    }
}
