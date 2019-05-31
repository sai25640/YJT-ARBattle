using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using  UnityEngine.UI;

namespace ARBattleTest
{
    public class TestZombieController : MonoBehaviour
    {
        private Vector3 TargetPoint;
        private float m_WalkSpeed = 0.2f;
        private Animator m_Anim;
        private NavMeshAgent m_Agent;
        public Text NameTxt;
        private StageSystem stageSystem;
        void Awake()
        {
            m_Anim = GetComponent<Animator>();
            m_Agent = GetComponent<NavMeshAgent>();
            GameObject targetPos = GameObject.Find("TargetPoint");
            TargetPoint = targetPos.transform.position;
            //stageSystem = GameObject.Find("TestLevelManager").GetComponent<StageSystem>();
        }

        void Start()
        {        
            m_Agent.SetDestination(TargetPoint);
            WalkAnim();
        }

        private void WalkAnim()
        {
            PlayAnim(3, "Walk", "WalkValue");
            m_Agent.speed = m_WalkSpeed;
        }

        private void PlayAnim(int clipCount, string triggerName, string floatName)
        {
            float rate = 1.0f / (clipCount - 1);
            m_Anim.SetTrigger(triggerName);
            m_Anim.SetFloat(floatName, rate * Random.Range(0, clipCount));
        }

        private void OnDestroy()
        {
            stageSystem.CountOfEnemyKilled++;
        }
    }
}
