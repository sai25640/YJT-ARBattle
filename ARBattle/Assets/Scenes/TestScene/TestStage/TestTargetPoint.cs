using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.AI;

namespace ARBattleTest
{
    public class TestTargetPoint : MonoBehaviour
    {
        public StageSystem stageSystem;
        void OnTriggerEnter(Collider col)
        {
            Debug.Log("OnTriggerEnter");
            if (col.tag == "Zombie")
            {
                col.transform.GetComponent<NavMeshAgent>().enabled = false;
                col.gameObject.DestroySelfAfterDelay(1f);      
            }
        }
    }
}
