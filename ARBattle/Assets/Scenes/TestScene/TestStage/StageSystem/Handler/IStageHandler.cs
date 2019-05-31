using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ARBattleTest
{
    public abstract class IStageHandler
    {
        protected StageSystem mStageSystem;
        protected IStageHandler mNextHandler;
        protected StageData mStageData;
        public IStageHandler(StageSystem stageSystem, StageData stageData)
        {
            mStageSystem = stageSystem;
            mStageData = stageData;
        }

        public IStageHandler SetNextHandler(IStageHandler handler)
        {
            mNextHandler = handler;
            return mNextHandler;
        }


        public void Handle(int level)
        {
            if (level == mStageData.Level)
            {
                UpdateStage();
                CheckIsFinished(); //检查关卡是否结束
            }
            else
            {
                //超过最大关卡数
                if (level>mStageSystem.MaxLevel )
                {
                    return;                
                }
                mNextHandler.Handle(level);
            }
        }

        protected virtual void UpdateStage()
        {
            mStageSystem.MaxEnemyCount = mStageData.Total;
        }

        private void CheckIsFinished()
        {
            if (mStageSystem.CountOfEnemyKilled >= mStageData.Total)
            {
                mStageSystem.EnterNextStage();           
            }
        }
    }
}
