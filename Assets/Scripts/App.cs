using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace ARBattle
{
    public class App : MonoBehaviour
    {
        void Start()
        {
            ResMgr.Init();

            UIMgr.SetResolution(1920, 1080, 0);

            //UIMgr.OpenPanel<LoginPanel>();
        }
    }
}