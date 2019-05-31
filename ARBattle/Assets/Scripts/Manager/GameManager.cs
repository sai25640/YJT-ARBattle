using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;

namespace ARBattle
{
    /// <summary>
    /// 管理整个游戏运行
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        ResLoader mResLoader = ResLoader.Allocate();

        private GameObject mARWorld;
        private GameObject mZombieA;
        public GameObject Player;
        private SocketServer mServer;
        private bool IsStartGame = false; //客户端是否已经开启过游戏

        private void Awake()
        {
            ResMgr.Init();
            Init();
        }

        public void Init()
        {
            ARManager.Instance.Init();
            //实例化ARWorld
            mARWorld = mResLoader.LoadSync<GameObject>("ARWorld").Instantiate();   
            ARWorld.Instance.Init();
            StageSystem.Instance.Init();
            mServer = mResLoader.LoadSync<GameObject>("SocketServer").Instantiate().transform.GetComponent<SocketServer>();
        }

        public void StartGame()
        {
            if (IsStartGame) return;

            //Player
            Player = mResLoader.LoadSync<GameObject>("GamePlayer").Instantiate();
            ARWorld.Instance.Show();
            StageSystem.Instance.Run();

            //开启UIPanel
            UIMgr.OpenPanel<GamePanel>();

            IsStartGame = true;
        }

        public void ReStartGame()
        {
            //一些清理工作
            Player.DestroySelfGracefully();

            IsStartGame = false;
            StartGame();
        }

        protected override void OnDestroy()
        {
            mARWorld.DestroySelfGracefully();
            Player.DestroySelfGracefully();
            mServer.DestroySelfGracefully();
            mResLoader.Recycle2Cache();
            mResLoader = null;

            base.OnDestroy();
        }

    }
}
