using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARShowHide : MonoBehaviour
{
    public GameObject[] Childs;
    void Awake()
    {
        EventCenter.AddListener(EventDefine.TargetFound, Show);
        EventCenter.AddListener(EventDefine.TargetLost, Hide);
    }

    void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.TargetFound, Show);
        EventCenter.RemoveListener(EventDefine.TargetLost, Hide);
    }

    public void Show()
    { 
        foreach (var go in Childs)
        {
            go.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (var go in Childs)
        {
            go.SetActive(false);
        }
    }
}

