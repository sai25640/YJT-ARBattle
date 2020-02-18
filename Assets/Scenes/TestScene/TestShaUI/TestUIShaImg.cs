using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using DG.Tweening;
namespace ARBattleTest
{
    public class TestUIShaImg : MonoBehaviour
    {
        private Image[] numImages = new Image[3];
        private int killNum = 0;
        private Sprite[] sprites;
        void Awake()
        {
            var tran = transform.GetChild(0);
            numImages[0] = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            numImages[1] = transform.GetChild(0).GetChild(1).GetComponent<Image>();
            numImages[2] = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        }

        void Start()
        {
            sprites = Resources.LoadAll<Sprite>("sha_num");
            numImages[0].gameObject.SetActive(true);
            numImages[1].gameObject.SetActive(false);
            numImages[2].gameObject.SetActive(false);      
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                killNum++;
                UpdateNumImage(killNum);
            }          
        }

        void UpdateNumImage(int num)
        {
            if (num <= 9)
            {
                numImages[0].gameObject.SetActive(true);
                numImages[1].gameObject.SetActive(false);
                numImages[2].gameObject.SetActive(false);

                numImages[0].transform.parent.localScale = Vector3.zero;
                numImages[0].transform.parent.DOLocalMove(new Vector3(0, -35, 0), 0f);
                numImages[0].sprite = sprites[num];
                numImages[0].transform.parent.DOScale(Vector3.one, 0.5f);
                numImages[0].transform.parent.DOLocalMove(new Vector3(75, 15, 0), 0.5f);
            }
            else if (num <= 99 && num > 9)
            {
                numImages[0].gameObject.SetActive(true);
                numImages[1].gameObject.SetActive(true);
                numImages[2].gameObject.SetActive(false);

                numImages[0].transform.parent.localScale = Vector3.zero;
                numImages[0].sprite = sprites[num/10];
                numImages[1].sprite = sprites[num%10];
                numImages[0].transform.parent.DOScale(Vector3.one, 0.5f);
            }
            else if (num <= 999 && num > 99)
            {
                numImages[0].gameObject.SetActive(true);
                numImages[1].gameObject.SetActive(true);
                numImages[2].gameObject.SetActive(true);

                numImages[0].transform.parent.localScale = Vector3.zero;
                numImages[0].sprite = sprites[num / 100];
                numImages[1].sprite = sprites[(num/10) % 10];
                numImages[2].sprite = sprites[num % 10];
                numImages[0].transform.parent.DOScale(Vector3.one, 0.5f);     
            }
            else
            {
                numImages[0].gameObject.SetActive(false);
                numImages[1].gameObject.SetActive(false);
                numImages[2].gameObject.SetActive(false);
                Debug.LogError("������UI�����õķ�Χ��1-999��");
            }
        }
    }
}
