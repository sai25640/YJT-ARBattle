using System.Collections;
using UnityEngine;

namespace ARBattle
{

    public delegate void HandAttack(bool isAtk);
    public delegate void HandSheild(bool isStart);

    public class mobileShake : MonoBehaviour
    {
        public static event HandAttack attack;
        public static event HandSheild shield;
        private bool gyinfo;
        private bool acinfo;

        //private float old_y = 0;
        //private float new_y = 0;
        //private float d_y = 0;
        //public float distance = 0.4f;
        //记录上一次的重力感应的Y值
        private float old_y = 0;

        //记录当前的重力感应的Y值
        private float new_y;

        //当前手机晃动的距离
        private float currentDistance = 0;

        //手机晃动的有效距离
        public float distance = 0.5f;

        private GUIStyle bb;

        private IEnumerator wait(float time)
        {
            yield return new WaitForSeconds(time);
            gyinfo = SystemInfo.supportsGyroscope;
            acinfo = SystemInfo.supportsAccelerometer;
        }

        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.gyro.enabled = true;
            bb = new GUIStyle();
            bb.normal.background = null; //这是设置背景填充的
            bb.normal.textColor = new Color(1, 0, 0); //设置字体颜色的
            bb.fontSize = 30; //当然，这是字体大小
            StartCoroutine(wait(1.0f));
        }

        private float timer = 0;
        //  private float gyro_y = 0;


        private bool isAtk = true;
        private bool isShield = true;

        // private bool isCalcTime_Fire;
        //private bool isCalcTime_Shield;

        private float mTime_Fire;
        private float mTime_Shield;


        private float time_RestShield = 0;

        private void Update()
        {
            // 检测isShield 超过秒数强制刷新
            if (!isShield)
            {
                time_RestShield += Time.deltaTime;
                if (time_RestShield >= 20f)
                {
                    isShield = true;
                    time_RestShield = 0;
                }
            }


            //触发逻辑
            if (acinfo && Input.acceleration.x >= 0.85f && isShield || Input.anyKeyDown)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    timer = 0;
                    isShield = false;
                    if (shield != null)
                        shield(true);
                    StartCoroutine(StartTimeDown_Shield(10f));
                    return;
                }
            }
            else
            {
                timer = 0;
            }



            //触发逻辑
            //if (acinfo && Input.acceleration.x >= -0.5f && Input.acceleration.x <= 0.5)
            {
                //timer += Time.deltaTime;
                new_y = Input.acceleration.y;
                currentDistance = new_y - old_y;
                old_y = new_y;

                if (currentDistance > distance && isAtk)
                {
                    isAtk = false;
                    StartCoroutine(StartTimeDown_Fire(0.4f));
#if UNITY_ANDROID && !UNITY_EDITOR
                Handheld.Vibrate();
#endif
                    if (attack != null)
                        attack(true);
                    //  isCalcTime_Fire = true;
                }



                //                new_y = Input.acceleration.x;
                //                d_y = new_y - old_y;
                //                old_y = new_y;
                //                if (d_y > distance && timer > 1)
                //                {
                //                    timer = 0;
                //                    if (isCalcTime_Fire)
                //                    {
                //                        StartCoroutine(StartTimeDown_Fire(0.5f));
                //                    }
                //                    else
                //                    {
                //#if UNITY_ANDROID && !UNITY_EDITOR
                //                Handheld.Vibrate();
                //#endif
                //                    attack?.Invoke();
                //                    isCalcTime_Fire = true;
                //                    }

                //                //进入冷却
                //                //冷却时间
                //                mTime_Fire = 1f;

                //                }
            }

        }

        IEnumerator StartTimeDown_Fire(float time)
        {
            yield return new WaitForSeconds(time);
            // isCalcTime_Fire = false;
            isAtk = true;
            if (attack != null)
                attack(false);
        }

        IEnumerator StartTimeDown_Shield(float time)
        {
            while (time > 0)
            {
                yield return new WaitForSeconds(1);
                time--;
                if (time == 2)
                {
                    Debug.Log("重置护盾.....");
                    if (shield != null)
                        shield(false);
                }

            }

            isShield = true;
        }

        public bool isShow = true;

        private void OnGUI()
        {
            if (!isShow) return;
            if (!gyinfo) GUI.Label(new Rect(700, 10, 100, 30), "陀螺仪启动失败", bb);
            if (!acinfo) GUI.Label(new Rect(700, 50, 100, 30), "重力感应仪启动失败", bb);
            GUI.Label(new Rect(50, 50, 200, 40), "陀螺仪", bb);
            GUI.Label(new Rect(50, 100, 200, 40), "X: " + Input.gyro.attitude.x.ToString("#0.0000"), bb);
            GUI.Label(new Rect(50, 150, 200, 40), "Y: " + Input.gyro.attitude.y.ToString("#0.0000"), bb);
            GUI.Label(new Rect(50, 200, 200, 40), "Z: " + Input.gyro.attitude.z.ToString("#0.0000"), bb);

            GUI.Label(new Rect(350, 50, 200, 40), "加速度", bb);
            GUI.Label(new Rect(350, 100, 200, 40), "X: " + Input.acceleration.x.ToString("#0.0000"), bb);
            GUI.Label(new Rect(350, 150, 200, 40), "Y: " + Input.acceleration.y.ToString("#0.0000"), bb);
            GUI.Label(new Rect(350, 200, 200, 40), "Z: " + Input.acceleration.z.ToString("#0.0000"), bb);
        }
    }
}