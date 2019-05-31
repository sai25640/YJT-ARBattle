using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ARBattle
{
    public class UISlider : MonoBehaviour
    {
        private Image BackgroundImage;
        private Image HealthImage;
      
        void Awake()
        {
            BackgroundImage = transform.GetChild(0).GetComponent<Image>();
            HealthImage = transform.GetChild(1).GetComponent<Image>();
            BackgroundImage.fillAmount = 1;
            HealthImage.fillAmount = 1;
        }

        public  bool DropOfBlood(float value)
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
    }
}
