using System.Collections;
using System.Collections.Generic;
using QFramework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ARBattleTest;
namespace ARBattle
{
    public  class GamePlayer : BaseCharacter
    {
        private Transform mFirePoint;
        private int fireIndex = 0;
        private int kickIndex = 0;
        private int laserIndex = 0;
        ResLoader mResLoader = ResLoader.Allocate();
        private UISlider hpSlider;
        private TestLaserShots laserShots;
        public override void Awake()
        {
            base.Awake();
            EventCenter.AddListener(EventDefine.AttackFire, AttackFire);
            EventCenter.AddListener(EventDefine.Kick, Kick);
            mFirePoint = transform.Find("FirePoint");

            InitAttr();
        }

        public void InitAttr()
        {
            GetComponent<CharacterAttr>().CurrentHP = 100;
            GetComponent<CharacterAttr>().IsKilled = false;       
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(EventDefine.AttackFire, AttackFire);
            EventCenter.RemoveListener(EventDefine.Kick, Kick);
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }

        private bool isGenerate = false;
        void Start()
        {
            //laserShots = mResLoader.LoadSync<GameObject>("PlasmaLasers").Instantiate().GetComponent<TestLaserShots>();
            //laserShots.transform.SetPositionAndRotation(mFirePoint.transform.position, mFirePoint.transform.rotation);
            //laserShots.SetChlidActive(false);

            Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                if (fireIndex > 0)
                {
                    mResLoader.LoadSync<GameObject>("Blue_Effect3").Instantiate()
                        .ApplySelfTo(self => self.transform.SetPositionAndRotation(mFirePoint.transform.position, mFirePoint.transform.rotation));
                    fireIndex--;
                }
                
                if (kickIndex>0)
                {
                    //Debug.Log(ARManager.Instance.ARCamera.transform.eulerAngles.y);
                    Quaternion quaternion = new Quaternion();
                    quaternion.eulerAngles = new Vector3(0,ARManager.Instance.ARCamera.transform.eulerAngles.y,0);
                    mResLoader.LoadSync<GameObject>("chongjibo").Instantiate()
                        .ApplySelfTo(self => self.transform.SetPositionAndRotation( new Vector3(mFirePoint.transform.position.x,-0.5f, mFirePoint.transform.position.z+3f), quaternion));
                    kickIndex--;
                }
             
                //if (laserIndex>0)
                //{
                //    if (!isGenerate)
                //    {
                //        laserShots.SetChlidActive(true);
                //        laserShots.GenerateLaser();
                //        isGenerate = true;
                //    }
                //    laserShots.SetFire(true);
                //    laserIndex--;
                //    Debug.Log("laserIndex:" + laserIndex);
                //    if (laserIndex == 0)
                //    {
                //        laserShots.SetFire(false);
                //        isGenerate = false;
                //        laserShots.SetChlidActive(false);
                //    }
                //}
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

            UpdateHpSlider(target.Attr.Attack);

            if (Attr.CurrentHP<=0)
            {
                Killed();
            }
        }

        public void AttackFire()
        {
            fireIndex++;
        }

        public void Kick()
        {
            kickIndex++;
        }

        public void AttachHpSlider(UISlider slider)
        {
            hpSlider = slider;
        }

        public void UpdateHpSlider(float attack)
        {
            hpSlider.DropOfBlood( attack / Attr.MaxHP);
        }
    }
}