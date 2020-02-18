using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ARBattle
{

    public class FSMSystem
    {
        private List<FSMState> mStates = new List<FSMState>();

        private FSMState mCurrentState;

        public FSMState currentState
        {
            get { return mCurrentState; }
        }

        public void AddState(params FSMState[] states)
        {
            foreach (FSMState s in states)
            {
                AddState(s);
            }
        }

        public void AddState(FSMState state)
        {
            if (state == null)
            {
                Debug.LogError("要添加的状态为空");
                return;
            }

            if (mStates.Count == 0)
            {
                mStates.Add(state);
                mCurrentState = state;
                mCurrentState.DoBeforeEntering();
                return;
            }

            foreach (FSMState s in mStates)
            {
                if (s.stateID == state.stateID)
                {
                    Debug.LogError("要添加的状态ID[" + s.stateID + "]已经添加");
                    return;
                }
            }

            mStates.Add(state);
        }

        public void DeleteState(StateID stateID)
        {
            if (stateID == StateID.NullState)
            {
                Debug.LogError("要删除的状态ID为空" + stateID);
                return;
            }

            foreach (FSMState s in mStates)
            {
                if (s.stateID == stateID)
                {
                    mStates.Remove(s);
                    return;
                }
            }

            Debug.LogError("要删除的StateID不存在集合中:" + stateID);
        }

        public void PerformTransition(Transition trans)
        {
            if (trans == Transition.NullTansition)
            {
                Debug.LogError("要执行的转换条件为空 ： " + trans);
                return;
            }

            StateID nextStateID = mCurrentState.GetOutPutState(trans);
            if (nextStateID == StateID.NullState)
            {
                Debug.LogError("在转换条件 [" + trans + "] 下，没有对应的转换状态");
                return;
            }

            foreach (FSMState s in mStates)
            {
                if (s.stateID == nextStateID)
                {
                    mCurrentState.DoBeforeLeaving();
                    mCurrentState = s;
                    mCurrentState.DoBeforeEntering();
                    return;
                }
            }
        }
    }
}
