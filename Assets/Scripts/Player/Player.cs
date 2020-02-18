using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using  UniRx;
using UnityEngine.Networking;

namespace ARBattle
{
    public class Player : NetworkBehaviour
    {
        public IntReactiveProperty Hp = new IntReactiveProperty(100);
        private  Transform mFirePoint;
        private int fireIndex = 0;
        ResLoader mResLoader = ResLoader.Allocate();
   
        private void Awake()
        {
            EventCenter.AddListener<int>(EventDefine.UpdateHP, UpdateHP);
            EventCenter.AddListener(EventDefine.AttackFire,AttackFire);
            mFirePoint = transform.Find("FirePoint");
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<int>(EventDefine.UpdateHP, UpdateHP);
            EventCenter.RemoveListener(EventDefine.AttackFire, AttackFire);
            mResLoader.Recycle2Cache();
           mResLoader = null;
        }

        void Start()
        {
            Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                if (fireIndex > 0)
                {
                    mResLoader.LoadSync<GameObject>("Blue_Effect3").Instantiate()
                        .ApplySelfTo(self => self.transform.SetPositionAndRotation(mFirePoint.transform.position, mFirePoint.transform.rotation));
                    fireIndex--;
                }
            }).AddTo(this);
        }

        public void UpdateHP(int count)
        {
            if (count < 0)
            {
                if (Hp.Value <= Mathf.Abs(count))
                {
                    //ËÀÍö
                    Hp.Value = 0;
                   EventCenter.Broadcast(EventDefine.ReStartGame);
                }
                else
                {
                    Hp.Value += count;
                }
            }
            else
            {
                Hp.Value += count;
            }
        }

        public void AttackFire()
        {
            fireIndex++;       
        }

    }
}
