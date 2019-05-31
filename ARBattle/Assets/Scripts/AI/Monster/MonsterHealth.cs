using System;
using System.Collections;
using System.Collections.Generic;
using ARBattle;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class MonsterHealth : MonoBehaviour
{
    private int hp =100;
    private Slider  hpSlider;
    private MonsterController Controller;
    void Awake()
    {
        hpSlider = transform.GetComponentInChildren<Slider>();
        EventCenter.AddListener<int, Collider>(EventDefine.Hit, Hit);
        Controller = GetComponent<MonsterController>();
    }

    void OnDestroy()
    {
        EventCenter.RemoveListener<int, Collider>(EventDefine.Hit, Hit);
    }

    void Hit(int num,Collider hit)
    {

        if (hit.gameObject.GetHashCode() != gameObject.GetHashCode())return;

        //�������˶���
        Controller.Hit();

        hp -= num;
        hpSlider.value = hp / 100f;
        if (hp<=0)
        {
            //������������
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
