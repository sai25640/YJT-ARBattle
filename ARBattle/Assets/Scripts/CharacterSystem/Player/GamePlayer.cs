using System.Collections;
using System.Collections.Generic;
using QFramework;
using UniRx;
using UnityEngine;

namespace ARBattle
{
    public  class GamePlayer : BaseCharacter
    {
        private Transform mFirePoint;
        private int fireIndex = 0;
        ResLoader mResLoader = ResLoader.Allocate();

        public override void Awake()
        {
            base.Awake();
            EventCenter.AddListener(EventDefine.AttackFire, AttackFire);
            mFirePoint = transform.Find("FirePoint");
        }

        private void OnDestroy()
        {
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

        public override void Attack(BaseCharacter target)
        {
         
        }

        public override void Killed()
        {
            Attr.IsKilled = true;
            EventCenter.Broadcast(EventDefine.ReStartGame);
        }

        public override void Move(BaseCharacter target)
        {
     
        }

        public override void UnderAttack(BaseCharacter target)
        {
            Attr.CurrentHP -= target.Attr.Attack;
            if (Attr.CurrentHP<=0)
            {
                Killed();
            }
        }

        public void AttackFire()
        {
            fireIndex++;
        }
    }
}