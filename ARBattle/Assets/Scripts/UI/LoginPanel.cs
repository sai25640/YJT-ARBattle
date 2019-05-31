using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ARBattle
{
    public class LoginPanel : MonoBehaviour
    {
        void Awake()
        {
            var armBtn = transform.Find("ArmBtn").GetComponent<Button>();
            var helmetBtn = transform.Find("HelmetBtn").GetComponent<Button>();

            armBtn.onClick.AddListener(() =>
            {
                //NetworkManager.singleton.StartHost();
                MainManager.Instance.LoginType = LoginType.Arm;
            });
            helmetBtn.onClick.AddListener(() =>
            {
                 //NetworkManager.singleton.StartClient();
                MainManager.Instance.LoginType = LoginType.Helmet;
            });
        }
    }
}
