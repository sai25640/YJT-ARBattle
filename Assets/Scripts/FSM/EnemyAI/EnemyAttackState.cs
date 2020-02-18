using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ARBattle
{
    public class EnemyAttackState : FSMState
    {
        private float mAttackTimer = 0;

        public EnemyAttackState(FSMSystem fsm, BaseCharacter c) : base(fsm, c)
        {
            mStateID = StateID.Attack;
            mAttackTimer = mCharacter.Attr.AtkRate;
        }

        public override void DoBeforeEntering()
        {
            //Debug.Log("EnemyAttackState Entering");
        }

        public override void DoBeforeLeaving()
        {
           // Debug.Log("EnemyAttackState Leaving");
        }

        public override void Reason(BaseCharacter target)
        {
            if (target == null)
            {
                mFSM.PerformTransition(Transition.LostSoldier);
            }
            else
            {
                float distance = Vector3.Distance(mCharacter.transform.position, target.transform.position);
                if (distance > mCharacter.Attr.AtkRange)
                {
                    mFSM.PerformTransition(Transition.LostSoldier);
                }
            }
        }

        public override void Act(BaseCharacter target)
        {
            if (target == null) return;
            mAttackTimer += Time.deltaTime;
            if (mAttackTimer >= mCharacter.Attr.AtkRate)
            {
                mCharacter.Attack(target);
                mAttackTimer = 0;
            }
        }
    }
}