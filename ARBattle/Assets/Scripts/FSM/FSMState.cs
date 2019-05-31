using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ARBattle
{
    public enum Transition
    {
        NullTansition = 0,
        CanAttack,
        LostSoldier,
        Hit
    }

    public enum StateID
    {
        NullState,
        Chase,
        Attack,
        UnderAttack
    }

    public abstract class FSMState
    {
        protected Dictionary<Transition, StateID> mMap = new Dictionary<Transition, StateID>();
        protected StateID mStateID;
        protected BaseCharacter mCharacter;
        protected FSMSystem mFSM;

        public FSMState(FSMSystem fsm, BaseCharacter character)
        {
            mFSM = fsm;
            mCharacter = character;
        }

        public StateID stateID
        {
            get { return mStateID; }
        }

        public void AddTransition(Transition trans, StateID id)
        {
            if (trans == Transition.NullTansition)
            {
                Debug.LogError("EnemyState Error: trans不能为空");
                return;
            }

            if (id == StateID.NullState)
            {
                Debug.LogError("EnemyState Error: 状态ID不能为空");
                return;
            }

            if (mMap.ContainsKey(trans))
            {
                Debug.LogError("EnemyState Error: " + trans + " 已经添加上了");
                return;
            }

            mMap.Add(trans, id);
        }

        public void DeleteTransition(Transition trans)
        {
            if (mMap.ContainsKey(trans) == false)
            {
                Debug.LogError("删除转换条件的时候， 转换条件：[" + trans + "]不存在");
                return;
            }

            mMap.Remove(trans);
        }

        public StateID GetOutPutState(Transition trans)
        {
            if (mMap.ContainsKey(trans) == false)
            {
                return StateID.NullState;
            }
            else
            {
                return mMap[trans];
            }
        }

        public virtual void DoBeforeEntering()
        {
        }

        public virtual void DoBeforeLeaving()
        {
        }

        public abstract void Reason(BaseCharacter target);
        public abstract void Act(BaseCharacter target);
    }
}
