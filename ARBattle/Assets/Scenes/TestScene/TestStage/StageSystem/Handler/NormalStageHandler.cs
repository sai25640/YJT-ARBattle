using System;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UnityEngine;

namespace ARBattleTest
{
    public class NormalStageHandler : IStageHandler
    {
        private int mSpawnTime = 2;         //出生间隔
        private float mSpawnTimer = 0;    
        private int mCountSpawned = 0;  //已经出生的怪物数量
        private int mEnemyArmyIndex = 0; //现在应该出生的怪物链下标，上一个种类出生完就index++
        private int mEnemyIndex = 0;
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
            mCountSpawned++;
            mEnemyIndex++;
            EnemyArmyData army = mStageData.EnemyArmys[mEnemyArmyIndex];
           mResLoader.LoadSync<GameObject>("TestZombieA").Instantiate()
                .ApplySelfTo(self =>
                {
                    self.transform.position = mStageSystem.GetRandomPos();
                    self.transform.GetComponent<TestZombieController>().NameTxt.text = army.Type;
                });

            if (mEnemyIndex >= army.Count)
            {
                mEnemyIndex = 0;
                mEnemyArmyIndex++;
            }
            //FactoryManager.enemyFactory.CreateCharacter
        }
    }
}