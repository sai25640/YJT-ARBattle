using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARBattle
{

    public class EnemyUnderAttackState : FSMState
    {
        private float timer = 0;
        private bool isHiting = false;
        public EnemyUnderAttackState(FSMSystem fsm, BaseCharacter c) :base(fsm, c)
        {
            mStateID = StateID.UnderAttack;
        }

        public override void DoBeforeEntering()
        {
            Debug.Log("EnemyUnderAttackState Entering");      
        }

        public override void DoBeforeLeaving()
        {
            Debug.Log("EnemyUnderAttackState Leaving");
        }

        public override void Reason(BaseCharacter target)
        {
            if (target == null) return;

            if (isHiting)
            {
                timer += Time.deltaTime;
                if (timer >= 0.5f)
                {
                    isHiting = false;
                    timer = 0;

                    float distance = Vector3.Distance(mCharacter.transform.position, target.transform.position);
                    if (distance <= mCharacter.Attr.AtkRange)
                    {
                        mFSM.PerformTransition(Transition.CanAttack);
                    }
                    else
                    {
                        mFSM.PerformTransition(Transition.LostSoldier);
                    }               
                }
            }     
        }

        public override void Act(BaseCharacter target)
        {
            if (target == null) return;

            if (isHiting == false)
            {
                isHiting = true;
                mCharacter.UnderAttack(target);
            }

         
        }

    }
}
