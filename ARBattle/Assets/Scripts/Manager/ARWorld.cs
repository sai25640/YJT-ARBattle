using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ARBattle
{
    /// <summary>
    /// AR场景管理，包括开始动画，显示隐藏世界，初始化怪物出生点等功能
    /// </summary>
    public class ARWorld : MonoSingleton<ARWorld>
    {
        List<Vector3> mSpawnPointList;
        ResLoader mResLoader = ResLoader.Allocate();
        private ARMeshRenderController mARMeshRenderController;
        private ARShowHide[] mArShowHides;
        public void Init()
        {
            InitSpawnPointList();    
            mARMeshRenderController = GetComponent<ARMeshRenderController>();
            mArShowHides = GetComponentsInChildren<ARShowHide>();
            Hide();
        }

        private void InitSpawnPointList()
        {
            mSpawnPointList = new List<Vector3>();
            int i = 1;
            while (true)
            {
                var point = GameObject.Find("SpawnPoint" + i); 
                if (point != null)
                {
                    i++;
                    mSpawnPointList.Add(new Vector3(point.transform.position.x, -0.5f, point.transform.position.z));//
                    point.gameObject.SetActive(false);
                }
                else
                {
                    break;
                }
            }
            //Debug.Log(mSpawnPointList.Count);
        }

        public List<Vector3> GetSpawnPointList()
        {
            if (mSpawnPointList.Count>0)
            {
                return mSpawnPointList;
            }
            else
            {
                Debug.LogError("SpawnPointList need init !");
                return null;
            }
        }

        public void Show()
        {
            mARMeshRenderController.Show();
            foreach (var arShowHide in mArShowHides)
            {
                arShowHide.Show();
            }
        }

        public void Hide()
        {
            mARMeshRenderController.Hide();
            foreach (var arShowHide in mArShowHides)
            {
                arShowHide.Hide();
            }
        }

        protected override void OnDestroy()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;

            base.OnDestroy();
        }
    }
}