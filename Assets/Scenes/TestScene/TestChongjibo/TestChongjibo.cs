using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ARBattleTest
{
    public class TestChongjibo : MonoBehaviour
    {
        public GameObject EffectOnCollision;
        private bool isFirst = true;

        void OnTriggerEnter(Collider col)
        {
            Debug.Log(col.name);
            //Vector3 hitPos = Vector3.one;
            //hitPos = col.ClosestPoint(transform.position);

            //if (isFirst)
            //{
            //    StartCoroutine(DelayInstantiateEffect(hitPos));
            //    isFirst = false;
            //}
        }

        IEnumerator DelayInstantiateEffect(Vector3 hitPos)
        {
            yield return new WaitForSeconds(1.8f);
            var instance = Instantiate(EffectOnCollision, hitPos, new Quaternion()) as GameObject;
        }

        void Update()
        {
            Debug.DrawLine(transform.forward ,transform.forward*10f);
        }
    }
}
