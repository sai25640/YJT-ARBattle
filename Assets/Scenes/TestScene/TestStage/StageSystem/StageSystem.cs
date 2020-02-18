using System;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UnityEngine;

namespace ARBattleTest
{
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
    }
    public class StageSystem : MonoBehaviour
    {
        int mLv = 1;
        List<Vector3> mPosList;
        IStageHandler mRootHandler;
        Vector3 mTargetPosition;
        int mCountOfEnemyKilled = 0;
        public int  MaxLevel = 0;
        public int MaxEnemyCount = 0;
        private GUIStyle guiStyle;
        private StageConfig mStageConfig;
   
        void Start()
        {
            LoadStageConfig("stage");//AppConsts.ConfigPath+
            InitPosition();

            //mFacade.RegisterObserver(GameEventType.EnemyKilled, new EnemyKilledObserverStageSystem(this));
#if UNITY_EDITOR
            guiStyle = new GUIStyle();
            guiStyle.normal.background = null; //这是设置背景填充的
            guiStyle.normal.textColor = new Color(0, 0, 1); //设置字体颜色的
            guiStyle.fontSize = 50; //当然，这是字体大小
#endif
        }

        void Update()
        { 
            mRootHandler.Handle(mLv);
        }

        private void InitPosition()
        {
            mPosList = new List<Vector3>();
            int i = 1;
            while (true)
            {
                GameObject go = GameObject.Find("SpawnPoint" + i);
                if (go != null)
                {
                    i++;
                    mPosList.Add(new Vector3(go.transform.position.x, -0.5f, go.transform.position.z));//
                    go.SetActive(false);
                }
                else
                {
                    break;
                }
            }

            //Debug.Log(mPosList.Count);
            GameObject targetPos = GameObject.Find("TargetPoint");
            mTargetPosition = targetPos.transform.position;
        }

        public Vector3 GetRandomPos()
        {
            return mPosList[UnityEngine.Random.Range(0, mPosList.Count)];
        }

        private void LoadStageConfig(string name)
        {
            StageConfig sc = new StageConfig();
            sc = SerializeHelper.LoadJsonFormResources<StageConfig>(name);
            InitStageChain(sc);//SerializeHelper.LoadJson<StageConfig>(path)
        }

        private void InitStageChain(StageConfig sc)
        {
            List<NormalStageHandler> handlers = new List<NormalStageHandler>();
            MaxLevel = sc.MaxLevel;
            for (int i = 0; i < sc.MaxLevel  ; i++)
            {
                NormalStageHandler handler= new NormalStageHandler(this, sc.Levels[i]);
                handlers.Add(handler);
            }

            for (int i = 0; i < handlers.Count-1; i++)
            {
                handlers[i].SetNextHandler(handlers[i + 1]);
            }

            mRootHandler = handlers[0];
        }

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

        public void EnterNextStage()
        {
            //Debug.Log("EnterNextStage");
            mLv++;
            mCountOfEnemyKilled = 0;
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(50, 50, 200, 50), "关卡数 :"+mLv, guiStyle);
            GUI.Label(new Rect(50, 100, 200, 50), "最大怪物数 :" + MaxEnemyCount, guiStyle);
            GUI.Label(new Rect(50, 150, 200, 50), "死亡怪物数 :" + mCountOfEnemyKilled, guiStyle);
#endif
        }

    }
}
