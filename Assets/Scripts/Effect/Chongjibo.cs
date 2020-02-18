using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARBattle
{
    public class Chongjibo : MonoBehaviour
    {
        public GameObject EffectOnCollision;
        public GameObject Left;
        public GameObject Right;
        private bool isFirst = false;
        private SphereCollider mCollider;
        public LayerMask CollidesWith = ~0;
        void Start()
        {
            mCollider = GetComponent<SphereCollider>();
            mCollider.enabled = false;
            StartCoroutine(DelayEnableCollider());
        }

        void OnTriggerEnter(Collider col)
        {
            Debug.Log(col.name);
 
            if (col.tag == "Enemy")
            {
                col.transform.GetComponent<BaseCharacter>().PerformTransition(Transition.Hit);
            }
            else if (col.tag == "Head")
            {
                Debug.Log("Hit Head");
                var character = col.transform.GetComponentInParent<BaseCharacter>();
                character.HitHead();
                character.PerformTransition(Transition.Hit);
            }
            else if (col.tag == "Guide" && StageSystem.Instance.Step2)
            {
                Debug.Log("Guide: Step2");
                col.transform.GetComponent<GuideEnemy>().UnderAttack();
            }
            else
            {
                Destroy(transform.parent.parent.gameObject, 2f);
                return;
            }

            Destroy(transform.parent.parent.gameObject, 2f);
        }

        IEnumerator DelayInstantiateEffect(Vector3 hitPos)
        {
            yield return new WaitForSeconds(1.8f);
            var instance = Instantiate(EffectOnCollision, hitPos, new Quaternion()) as GameObject;
        }

        IEnumerator DelayEnableCollider()
        {
            yield return new WaitForSeconds(0.5f);
            mCollider.enabled = true;
            isFirst = true;
        }

        IEnumerator DelayEnableEffect()
        {
            yield return new WaitForSeconds(0.5f);
            isFirst = true;
        }

        void Update()
        {
            //Debug.DrawLine(transform.forward, transform.forward * 3f);
            if (isFirst)
            {
                RaycastDetection(transform);

                RaycastDetection(Left.transform);

                RaycastDetection(Right.transform);
            }
        }

        private void RaycastDetection(Transform t)
        {           
            RaycastHit hit;
            if (Physics.Raycast(t.position, t.forward, out hit, 1, CollidesWith))
            {
                if (hit.collider.tag == "Enemy" || hit.collider.tag == "Head")
                {
                    var instance = Instantiate(EffectOnCollision, hit.point, Quaternion.identity) as GameObject;
                    isFirst = false;
                    Debug.Log(t.gameObject.name);
                }
                Destroy(t.parent.parent.gameObject, 2f);
                return;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1f);
            //Gizmos.DrawLine(Left.transform.position, Left.transform.position + Left.transform.forward * 2f);
            //Gizmos.DrawLine(Right.transform.position, Right.transform.position + Right.transform.forward * 2f);
        }
    }

}
