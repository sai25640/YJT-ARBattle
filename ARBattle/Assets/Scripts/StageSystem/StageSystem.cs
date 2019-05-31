using System;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UnityEngine;

namespace ARBattle
{
    #region 关卡配置类

    public class StageConfig
    {
        public int MaxLevel;
        public List<StageData> Levels = new List<StageData>();
    }
    public class StageData
    {
        public int Level;    //关卡等级
        public int Total;   //关卡怪物总数
        public List<EnemyArmyData> EnemyArmys; //怪物种类及对应数量
    }
    public class EnemyArmyData
    {
        public string Type;
        public int Count;
        public string PrefabName;
    }

    #endregion

    public class StageSystem : MonoSingleton<StageSystem>
    {
        #region 字段
        int mLv = 1;
        IStageHandler mRootHandler;
        private List<IStageHandler> mHandlers = new List<IStageHandler>();
       Vector3 mTargetPosition;
        int mCountOfEnemyKilled = 0;
        public int MaxLevel = 0;
        public int MaxEnemyCount = 0;
        private GUIStyle guiStyle;
        private StageConfig mStageConfig;
        private bool IsRun = false;  //是否开始跑关卡
        public List<GameObject> EnemyList = new List<GameObject>();
        #endregion

        #region 属性
        public int CountOfEnemyKilled
        {
            get
            {
                return mCountOfEnemyKilled;

            }
            set
            {
                mCountOfEnemyKilled = value;
            }
        }


        #endregion

        #region 公有方法
        public void Init()
        {
            LoadStageConfig("stage");
            InitGUI();
        }

        public void Run(int level=1)
        {
            Reset();
            mLv = level;
            IsRun = true;
        }

        public void Stop()
        {
            IsRun = false;   
        }

        public void EnterNextStage()
        {
            //Debug.Log("EnterNextStage");
            mLv++;
            mCountOfEnemyKilled = 0;
        }

        public Vector3 GetRandomSpawnPoint()
        {
            var list = ARWorld.Instance.GetSpawnPointList();
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public void ClearEnemyList()
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].DestroySelfGracefully();
            }
            EnemyList.Clear();
        }
        #endregion

        #region 私有方法
        void Update()
        {
            if (!IsRun) return;
            mRootHandler.Handle(mLv);
        }

        private void LoadStageConfig(string name)
        {
            StageConfig sc = new StageConfig();
            sc = SerializeHelper.LoadJsonFormResources<StageConfig>(name);
            InitStageChain(sc);
        }

        private void InitStageChain(StageConfig sc)
        {
            MaxLevel = sc.MaxLevel;
            for (int i = 0; i < sc.MaxLevel; i++)
            {
                var handler = new NormalStageHandler(this, sc.Levels[i]);
                mHandlers.Add(handler);
            }

            for (int i = 0; i < mHandlers.Count - 1; i++)
            {
                mHandlers[i].SetNextHandler(mHandlers[i + 1]);
            }
            mRootHandler = mHandlers[0];
        }

        private void InitGUI()
        {
#if UNITY_EDITOR
            guiStyle = new GUIStyle();
            guiStyle.normal.background = null; //这是设置背景填充的
            guiStyle.normal.textColor = new Color(0, 0, 1); //设置字体颜色的
            guiStyle.fontSize = 50; //当然，这是字体大小
#endif
        }


        private void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(50, 50, 200, 50), "关卡数 :" + mLv, guiStyle);
            GUI.Label(new Rect(50, 100, 200, 50), "最大怪物数 :" + MaxEnemyCount, guiStyle);
            GUI.Label(new Rect(50, 150, 200, 50), "死亡怪物数 :" + mCountOfEnemyKilled, guiStyle);
#endif
        }

        private void Reset()
        {
            for (int i = 0; i < mHandlers.Count; i++)
            {
                mHandlers[i].Reset();
            }

            ClearEnemyList();
        }
        #endregion


    }
}
