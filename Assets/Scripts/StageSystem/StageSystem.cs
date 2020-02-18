using System;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UniRx;
using UnityEngine;

namespace ARBattle
{
    #region 关卡配置类

    public class StageConfig
    {
        public int MaxLevel;
        public List<StageData> Levels = new List<StageData>();
        
        //根据关卡等级获取关卡时间
        public float GetStageTimeByLevel(int num)
        {
            return Levels[num-1].Total * Levels[num-1].SpawnTime;
        }
    }
    public class StageData
    {
        public int Level;    //关卡等级
        public int Total;   //关卡怪物总数
        public float SpawnTime; //关卡产怪间隔时间
        public List<EnemyArmyData> EnemyArmys; //怪物种类及对应数量
    }
    public class EnemyArmyData
    {
        public string Type;
        public int Count;
        public string[] Prefabs;
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
        public bool IsUpdateStageTime = true;

        //新手指导相关
        public IntReactiveProperty GuideStep = new IntReactiveProperty(0);  //新手指导关卡计步器
        ResLoader mResLoader = ResLoader.Allocate();
        Vector3 GuidePos = new Vector3(0, -0.5f, 2.7f);
        public bool Step1 = true;
        public bool Step2 = false;
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
                EventCenter.Broadcast(EventDefine.UpdateUISha);    
            }
        }

        #endregion

        #region 公有方法
        public void Init()
        {
            LoadStageConfig("stage");
            InitGUI();
        }

        /// <summary>
        /// 开启新手指导关卡
        /// </summary>
         void StartGuide()
        {
            Step1 = true;
            Step2 = false;

            mResLoader.LoadSync<GameObject>("ZombieGuide").Instantiate()
                    .ApplySelfTo(self =>
                    {
                        self.transform.position = GuidePos;
                        EnemyList.Add(self);
                    });
        }

        public void NextStep()
        {
            GuideStep.Value++;     
            UIMgr.GetPanel<GuidePanel>().NextStep();
            Step1 = false;
            Step2 = true;
            //结束新手指导模式，进入正式关卡
            if (GuideStep.Value>=2)
            {
                UIMgr.ClosePanel<GuidePanel>();
                UIMgr.OpenPanel<GamePanel>();
                Run(1);
            }
        }

        public void Run(int level=0)
        {
            ClearEnemyList();

            if (level == 0)  //新手指导模式
            {
                StartGuide();
            }
            else
            {
                Reset();
                mLv = level;
                //IsRun = true;
            }


        }

        /// <summary>
        /// 允许更新关卡
        /// </summary>
        public void Play()
        {
            IsRun = true;
        }

        /// <summary>
        /// 停止更新关卡
        /// </summary>
        public void Stop()
        {
            IsRun = false;   
        }

        public void EnterNextStage()
        {
            //Debug.Log("EnterNextStage");
            mLv++;
            mCountOfEnemyKilled = 0;

            if (mLv<= MaxLevel)
            {
                Stop();
                EventCenter.Broadcast(EventDefine.TimeDown);
            }      
        }

        public float GetStageTime()
        {
            return mStageConfig.GetStageTimeByLevel(mLv);
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
            mStageConfig = new StageConfig();
            mStageConfig = SerializeHelper.LoadJsonFormResources<StageConfig>(name);
            InitStageChain(mStageConfig);
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
      
            //可以更新关卡时间
            IsUpdateStageTime = true;

            GuideStep.Value = 0;
        }
        #endregion


    }
}
