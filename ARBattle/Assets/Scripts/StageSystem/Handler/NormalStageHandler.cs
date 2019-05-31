using System;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UnityEngine;

namespace ARBattle
{
    public class NormalStageHandler : IStageHandler
    {
        private int mSpawnTime = 2;         //出生间隔
        private float mSpawnTimer = 0;    
        ResLoader mResLoader = ResLoader.Allocate();

        public NormalStageHandler(StageSystem stageSystem, StageData stageData) : base(stageSystem, stageData)
        {
            mSpawnTimer = mSpawnTime;
        }

        protected override void UpdateStage()
        {
            base.UpdateStage();
            if (mCountSpawned < mStageData.Total)
            {
                mSpawnTimer -= Time.deltaTime;
                if (mSpawnTimer <= 0)
                {
                    SpawnEnemy();
                    mSpawnTimer = mSpawnTime;
                }
            }
        }

        void SpawnEnemy()
        {
            try
            {
                mCountSpawned++;
                mEnemyIndex++;
                EnemyArmyData army = mStageData.EnemyArmys[mEnemyArmyIndex];
                mResLoader.LoadSync<GameObject>(army.PrefabName).Instantiate()
                    .ApplySelfTo(self =>
                    {
                        self.transform.position = mStageSystem.GetRandomSpawnPoint();
                        mStageSystem.EnemyList.Add(self);
                    });

                if (mEnemyIndex >= army.Count)
                {
                    mEnemyIndex = 0;
                    mEnemyArmyIndex++;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("检查下，可能是配置表中total和enemyArmys怪物总数不匹配！");
            }
          
            //FactoryManager.enemyFactory.CreateCharacter
        }

        void OnDestroy()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
    }
}