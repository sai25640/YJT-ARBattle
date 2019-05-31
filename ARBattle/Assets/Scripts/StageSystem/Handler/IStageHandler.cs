using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ARBattle
{
    public abstract class IStageHandler
    {
        protected StageSystem mStageSystem;
        protected IStageHandler mNextHandler;
        protected StageData mStageData;
        protected int mCountSpawned = 0;  //已经出生的怪物数量
        protected int mEnemyArmyIndex = 0; //现在应该出生的怪物链下标，上一个种类出生完就index++
        protected int mEnemyIndex = 0;
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
                if (level > mStageSystem.MaxLevel )
                {
                    mStageSystem.Stop();
                    EventCenter.Broadcast(EventDefine.ReStartGame);
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

        public void Reset()
        {
            mCountSpawned = 0;
            mEnemyIndex = 0;
            mEnemyArmyIndex = 0;
        }
    }
}
