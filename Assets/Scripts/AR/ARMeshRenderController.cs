using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARBattle
{
    public class ARMeshRenderController : MonoBehaviour
    {
        private MeshRenderer[] mMeshRenderers;
        private SkinnedMeshRenderer[] mSkinnedMeshRenderers;

        void Awake()
        {
            mMeshRenderers = GetComponentsInChildren<MeshRenderer>();
            mSkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            EventCenter.AddListener(EventDefine.TargetFound,Show);
            EventCenter.AddListener(EventDefine.TargetLost, Hide);
        }

        void OnDestroy()
        {
            EventCenter.RemoveListener(EventDefine.TargetFound, Show);
            EventCenter.RemoveListener(EventDefine.TargetLost, Hide);
        }

        public void Show()
        {
            foreach (var renderer in mMeshRenderers)
            {
                if (renderer.tag != "Wall")
                {
                    renderer.enabled = true;
                }            
            }

            foreach (var renderer in mSkinnedMeshRenderers)
            {
                renderer.enabled = true;
            }
        }

        public void Hide()
        {
            foreach (var renderer in mMeshRenderers)
            {
                renderer.enabled = false;
            }

            foreach (var renderer in mSkinnedMeshRenderers)
            {
                renderer.enabled = false;
            }
        }
    }
}
