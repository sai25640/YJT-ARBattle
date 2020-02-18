using System;
using System.Collections;
using System.Collections.Generic;
using ARBattle;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class ZombieHealth : MonoBehaviour
{
    private int hp =100;
    private Slider  hpSlider;
    private ZombieController Controller;
    void Awake()
    {
        hpSlider = transform.GetComponentInChildren<Slider>();
        EventCenter.AddListener<int, Collider>(EventDefine.Hit, Hit);
        Controller = GetComponent<ZombieController>();
    }

    void OnDestroy()
    {
        EventCenter.RemoveListener<int, Collider>(EventDefine.Hit, Hit);
    }

    void Hit(int num,Collider hit)
    {

        if (hit.gameObject.GetHashCode() != gameObject.GetHashCode())return;

        //播放受伤动画
        Controller.Hit();

        hp -= num;
        hpSlider.value = hp / 100f;
        if (hp<=0)
        {
            //播放死亡动画
            Controller.Death();
            StageSystem.Instance.CountOfEnemyKilled++;
            hpSlider.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(_ =>
            {
                gameObject.DestroySelfGracefully();
            }).AddTo(this);
        }
    }
}
