using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ARBattle
{
    public class EnemyChaseState : FSMState
    {
        public EnemyChaseState(FSMSystem fsm, BaseCharacter c) : base(fsm, c)
        {
            mStateID = StateID.Chase;
        }

        public override void DoBeforeEntering()
        {
            Debug.Log("EnemyChaseState Entering");                
        }

        public override void DoBeforeLeaving()
        {
            Debug.Log("EnemyChaseState Leaving");
        }

        public override void Reason(BaseCharacter target)
        {
            if (target==null) return;

            float distance = Vector3.Distance(mCharacter.transform.position, target.transform.position);
            if (distance <= mCharacter.Attr.AtkRange)
            {
                mFSM.PerformTransition(Transition.CanAttack);
            }
        }

        public override void Act(BaseCharacter target)
        {
            if (target == null) return;            
            mCharacter.Move(target);         
        }
    }
}
