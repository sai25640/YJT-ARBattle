using QFramework;
using UnityEngine.Networking;

namespace ARBattle
{
    public enum LoginType
    {
        Arm,        //手臂端
        Helmet,   //头盔端
        Server    //服务器端
    }
    /// <summary>
    /// 程序的主入口，管理登录哪个端
    /// </summary>
    public class MainManager : MonoSingleton<MainManager>
    {
        private LoginType loginType = LoginType.Helmet;

        public LoginType LoginType
        {
            get
            {
                return loginType;
            }

            set
            {
                loginType = value;
            }
        }

        private void Awake()
        {
            this.DontDestroyOnLoad();
        }

        void Start()
        {
            //NetworkManager.singleton.networkAddress = "192.168.1.107";
            //NetworkManager.singleton.networkPort = 8899;
            //Log.I(NetworkManager.singleton.networkAddress);
            //Log.I(NetworkManager.singleton.networkPort);
        }

        protected override void OnDestroy()
        {
            Log.I("Main OnDestroy");
            base.OnDestroy();
        }
    }
}
