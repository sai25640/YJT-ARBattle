using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using QFramework;

namespace ARBattle
{
    public class MonsterController : MonoBehaviour
    {
        private Transform m_Target;
        private float m_WalkSpeed = 0.1f;
        private float m_RunSpeed = 0.2f;
        // 达到攻击的距离
        private float m_DistanceJudge = 0.5f;
        private float m_HitDealyTime = 2f;

        private Animator m_Anim;
        private NavMeshAgent m_Agent;

        // 攻击时间间隔
        private float m_AttackInterval = 2.5f;
        private bool m_IsFirstAttack = true;
        private float m_Timer = 0;
        private bool m_IsAttacking = false;
        private bool m_IsDeath = false;
        public bool IsDeath
        {
            get
            {
                return m_IsDeath;
            }
        }
        private bool m_IsHitting = false;
        void Start()
        {
            m_Target = GameManager.Instance.Player.transform;
            m_Anim = GetComponent<Animator>();
            m_Agent = GetComponent<NavMeshAgent>();

            m_Agent.SetDestination(m_Target.position);
            RandomWalkOrRun();
           
            AudioManager.Instance.SendMsg(new AudioSoundMsg("zombie_growl_2"));

            Observable.EveryFixedUpdate()
             .Subscribe(_ =>
             {
                 if (!m_Agent)return;
                 if (!m_Target) return;

                 Vector3 targetPos = new Vector3(m_Target.position.x, transform.position.y, m_Target.position.z);
                 //进入攻击范围
                 if (Vector3.Distance(transform.position, targetPos) < m_DistanceJudge)
                 {
                     if (m_Agent.isStopped == false)
                     {
                         //结束移动，准备开始攻击
                         m_Agent.isStopped = true;
                     }
                     if (m_IsAttacking == false)
                     {
                         m_Timer += Time.deltaTime;
                         if (m_Timer >= m_AttackInterval)
                         {
                             m_Timer = 0.0f;
                             Attack();
                         }
                     }
                     if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("AttackBlendTree") == false)
                     {
                         if (m_IsFirstAttack)
                         {
                             m_DistanceJudge += 0.15f;
                             m_IsFirstAttack = false;
                             Attack();
                         }
                         else
                         {
                             m_IsAttacking = false;
                         }
                     }
                 }
                 else//脱离了攻击范围，开始追逐
                 {
                     if (m_Agent.isStopped)
                     {
                         m_DistanceJudge -= 0.15f;
                         m_Agent.isStopped = false;
                         m_IsFirstAttack = true;
                         RandomWalkOrRun();
                     }
                     m_Agent.SetDestination(m_Target.position);
                 }
             })
            .AddTo(this);
        }

        public void Death()
        {
            if (m_IsDeath) return;

            GetComponent<CapsuleCollider>().enabled = false;
            PlayAnim(1, "Death", "DeathValue");
            m_Agent.isStopped = true;
            m_IsDeath = true;
            Destroy(m_Agent);
            //EventCenter.Broadcast(EventDefine.ZombieDeath);
        }

        public void HitLeft()
        {
            m_Anim.SetTrigger("HitLeft");
            m_Agent.isStopped = true;
            m_Anim.SetTrigger("Idle");
            m_IsHitting = true;
            StartCoroutine(HitDealy());
        }

        public void HitRight()
        {
            m_Anim.SetTrigger("HitRight");
            m_Agent.isStopped = true;
            m_Anim.SetTrigger("Idle");
            m_IsHitting = true;
            StartCoroutine(HitDealy());
        }

        public void Hit()
        {
            if (m_IsDeath)return;
   
            PlayAnim(1, "Hit", "HitValue");
            m_Agent.isStopped = true;
            m_Anim.SetTrigger("Idle");
            m_IsHitting = true;
            StartCoroutine(HitDealy());

            AudioManager.Instance.SendMsg(new AudioSoundMsg("zombie_attack_1"));
        }
        IEnumerator HitDealy()
        {    
            yield return new WaitForSeconds(m_HitDealyTime);
            if (m_Agent)
            {
                m_Agent.isStopped = false;
                m_IsHitting = false;
                RandomWalkOrRun();
            }         
        }
        private void RandomWalkOrRun()
        {
            int random = Random.Range(0, 2);
            if (random ==0)
            {
                WalkAnim();      
            }
            else
            {
                RunAnim();         
            }
        }

        private void WalkAnim()
        {
            PlayAnim(1, "Walk", "WalkValue");
            m_Agent.speed = m_WalkSpeed;
        }

        private void RunAnim()
        {
            PlayAnim(1, "Run", "RunValue");
            m_Agent.speed = m_RunSpeed;
        }

        private void Attack()
        {
            Vector3 targetPos = new Vector3(m_Target.position.x, transform.position.y, m_Target.position.z);
            transform.LookAt(targetPos);
            PlayAnim(2, "Attack", "AttackValue");
            m_IsAttacking = true;

            EventCenter.Broadcast(EventDefine.UpdateHP, -10);
            EventCenter.Broadcast(EventDefine.ScreenBlood);

            AudioManager.Instance.SendMsg(new AudioSoundMsg("zombie_attack_3"));

        }

        private void PlayAnim(int clipCount,string triggerName,string floatName)
        {
            float rate = 1.0f / (clipCount - 1);
            m_Anim.SetTrigger(triggerName);
            m_Anim.SetFloat(floatName, rate * Random.Range(0, clipCount));
        }

    }
}