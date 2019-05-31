using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideUIBase : MonoBehaviour
{

    public UnityEngine.UI.Image BackgroundImage;
    public UnityEngine.UI.Image HealthImage;
    public UnityEngine.UI.Text msgText;

    protected void Awake()
    {
        BackgroundImage = this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        HealthImage = this.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
        msgText = this.transform.GetChild(2).GetComponent<Text>();
    }
    public virtual void Init()
    {
        BackgroundImage.fillAmount = 1;
        HealthImage.fillAmount = 1;
    }
    public virtual void Init(int value,bool isAll = true)
    {

    }

    public void ChangeHpText(string msg)
    {
        msgText.text = msg;
    }

    public virtual bool DropOfBlood(float value)
    {
        Debug.Log(value);
        if (BackgroundImage.fillAmount != HealthImage.fillAmount)
        {
            BackgroundImage.fillAmount = HealthImage.fillAmount;
        }
        if (value > HealthImage.fillAmount)
        {
            value = HealthImage.fillAmount;
        }
        HealthImage.fillAmount -= value;
        if (HealthImage.fillAmount == 0)
        {
            BackgroundImage.DOFillAmount(0, 0.5f);
            return true;
        }
        BackgroundImage.DOFillAmount(BackgroundImage.fillAmount - value, 0.5f);
        return false;
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DropOfBlood(0.2f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Init();
        }

    }

}
