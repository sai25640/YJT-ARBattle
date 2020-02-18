using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UniRx;
namespace ARBattleTest
{
    public class LevelData
    {
        public IntReactiveProperty LevelNumber = new IntReactiveProperty(1);
        public  FloatReactiveProperty SpawnRate = new FloatReactiveProperty(2f);
        public IntReactiveProperty EnemyTotalCount = new IntReactiveProperty(0);
        public List<LevelEnemyData> EnemyDataList = new List<LevelEnemyData>();

        public LevelData(int level, int rate, List<LevelEnemyData> list)
        {
            LevelNumber.Value = level;
            SpawnRate.Value = rate;
            EnemyDataList = list;
            EnemyTotalCount.Value = TotalCount(list);
        }

        private int TotalCount(List<LevelEnemyData> list)
        {
            int total = 0;
            foreach (var data in list)
            {
                total += data.EnemyCount.Value;
            }
            return total;
        }
    }

    public class LevelEnemyData
    {
        public  StringReactiveProperty EnemyType = new StringReactiveProperty();
        public IntReactiveProperty EnemyCount = new IntReactiveProperty(0);

        public LevelEnemyData(string type, int count)
        {
            EnemyType.Value = type;
            EnemyCount.Value = count;
        }
    }

    public class TestLevelManager : MonoBehaviour
    {
        public Transform[] SpawnPoints;
        public Transform TargetPoint;
        public GameObject  TestZombiePrefab;
        private List<LevelData> LevelDatas = new List<LevelData>();
        private  IntReactiveProperty CurrentLevel = new IntReactiveProperty(1);
      
        void Start()
        {
            //LevelDatas = LoadLevelData();
            //StartGame(CurrentLevel.Value);
            ResMgr.Init();
        }

        //读取关卡配置文档
        private List<LevelData> LoadLevelData()
        {
            List<LevelData> levelDatas = new List<LevelData>();

            List<LevelEnemyData> list1 = new List<LevelEnemyData>();
            list1.Add(new LevelEnemyData("普通怪",15));
            LevelData level1 = new LevelData(1,2,list1);

            List<LevelEnemyData> list2 = new List<LevelEnemyData>();
            list2.Add(new LevelEnemyData("远程怪", 10));
            list2.Add(new LevelEnemyData("精英怪", 5));
            LevelData level2 = new LevelData(2, 2, list2);

            levelDatas.Add(level1);
            levelDatas.Add(level2);

            return levelDatas;
        }

        //根据关卡配置文档生成怪物
        private void StartGame(int level)
        {
            LevelData data = LevelDatas[level];
           
        }


    }
}
