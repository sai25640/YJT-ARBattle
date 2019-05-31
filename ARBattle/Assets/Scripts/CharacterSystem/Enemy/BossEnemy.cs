using System;
using MalbersAnimations.Utilities;
using QFramework;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ARBattle
{
    public enum EffectID
    {
        FireBall = 111,
        FireBreath
    }
    [RequireComponent(typeof(AnimationModel))]
    public class BossEnemy : BaseCharacter
    {
        protected Animator mAnim;
        protected NavMeshAgent mAgent;
        protected BaseCharacter mTarget;
        protected AnimationModel mAniModel;
        private UISlider hpSlider;
        protected EffectManager mEffectManager;
        public override void Awake()
        {
            base.Awake();
     
            mAnim = GetComponent<Animator>();
            mAgent = GetComponent<NavMeshAgent>();
            mAniModel = GetComponent<AnimationModel>();
            hpSlider = transform.GetComponentInChildren<UISlider>();
            mTarget = GameManager.Instance.Player.GetComponent<GamePlayer>();
            mEffectManager = GetComponent<EffectManager>();
            MakeFSM();
        }

        private void MakeFSM()
        {
            mFSMSystem = new FSMSystem();

            EnemyChaseState chaseState = new EnemyChaseState(mFSMSystem, this);
            chaseState.AddTransition(Transition.CanAttack, StateID.Attack);
            chaseState.AddTransition(Transition.Hit, StateID.UnderAttack);

            EnemyAttackState attackState = new EnemyAttackState(mFSMSystem, this);
            attackState.AddTransition(Transition.LostSoldier, StateID.Chase);
            attackState.AddTransition(Transition.Hit, StateID.UnderAttack);

            EnemyUnderAttackState underAttackState = new EnemyUnderAttackState(mFSMSystem, this);
            underAttackState.AddTransition(Transition.LostSoldier, StateID.Chase);
            underAttackState.AddTransition(Transition.Hit, StateID.UnderAttack);
            underAttackState.AddTransition(Transition.CanAttack, StateID.Attack);

            mFSMSystem.AddState(chaseState, attackState,underAttackState);
        }

        void Update()
        {
            UpdateFSMAI(mTarget);
        }

        public void UpdateFSMAI(BaseCharacter target)
        {
            if (target==null || Attr.IsKilled) return;
            mFSMSystem.currentState.Reason(target);
            mFSMSystem.currentState.Act(target);
        }

        public override void Attack(BaseCharacter target)
        {
            transform.LookAt(target.transform.position);
            PlayBossAnim(mAniModel.AttackCount, "Attack", "AttackValue");
            mAgent.isStopped = true;
            target.UnderAttack(this);     
 
            EventCenter.Broadcast(EventDefine.ScreenBlood);
        }

        public override void Killed()
        {
            if (Attr.IsKilled) return;

            GetComponent<CapsuleCollider>().enabled = false;
            PlayAnim(mAniModel.DeathCount, "Death", "DeathValue");
            mAgent.isStopped = true;
            Attr.IsKilled = true;
            Destroy(mAgent);

            StageSystem.Instance.CountOfEnemyKilled++;
            hpSlider.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(_ =>
            {
                gameObject.DestroySelfGracefully();
            }).AddTo(this);
        }

        private bool isFirst = true;
        public override void Move(BaseCharacter target)
        {
            if (isFirst)
            {
                PlayAnim(mAniModel.MoveCount, "Walk", "WalkValue");
                mAgent.speed = Attr.MoveSpeed;
                isFirst = false;
                //AudioManager.Instance.SendMsg(new AudioSoundMsg("zombie_growl_2"));
            }
            if (mAnim.GetCurrentAnimatorStateInfo(0).IsName("WalkBlendTree") == false )
            {
                PlayAnim(mAniModel.MoveCount, "Walk", "WalkValue");
            }
            mAgent.isStopped = false;
            mAgent.SetDestination(target.transform.position);
        }

        public override void UnderAttack(BaseCharacter target)
        {
            if (target.Attr.IsKilled) return;
            PlayAnim(mAniModel.HitCount, "Hit", "HitValue");
            mAgent.isStopped = true;

            Attr.CurrentHP -= target.Attr.Attack;
            //hpSlider.value = (float)Attr.CurrentHP/ Attr.MaxHP;
            hpSlider.DropOfBlood((float) target.Attr.Attack / Attr.MaxHP);
            AudioManager.Instance.SendMsg(new AudioSoundMsg("zombie_attack_1"));

            if (Attr.CurrentHP<=0)
            {
                Killed();
            }
        }

        private void PlayAnim(int clipCount, string triggerName, string floatName)
        {
            float rate = 1.0f / (clipCount - 1);
            //mAnim.CrossFade("Run");
            mAnim.SetTrigger(triggerName);
            mAnim.SetFloat(floatName, rate * Random.Range(0, clipCount));
        }

        private void PlayBossAnim(int clipCount, string triggerName, string floatName)
        {
            float rate = 1.0f / (clipCount - 1);
            //mAnim.CrossFade("Run");
            mAnim.SetTrigger(triggerName);
            int r = Random.Range(0, clipCount);
            mAnim.SetFloat(floatName, rate * r);
            if (r == 0)
            {
                mEffectManager.PlayEffect((int)EffectID.FireBall);
                AudioManager.Instance.SendMsg(new AudioSoundMsg("FireExit"));
            }
            else
            {
                mEffectManager.PlayEffect((int)EffectID.FireBreath);
                AudioManager.Instance.SendMsg(new AudioSoundMsg("FireExplosion2"));
            }
        }
    }
}
