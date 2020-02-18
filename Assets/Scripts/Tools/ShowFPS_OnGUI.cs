using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS_OnGUI : MonoBehaviour {

    public float fpsMeasuringDelta = 2.0f;

    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;
    GUIStyle guiStyle = new GUIStyle();
    private void Start()
    {
        timePassed = 0.0f;

        guiStyle.normal.background = null;    //�������ñ�������
        guiStyle.normal.textColor = new Color(1.0f, 0.5f, 0.0f);   //����������ɫ��
        guiStyle.fontSize = 40;       //��Ȼ�����������С
    }

    private void Update()
    {
        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;

            timePassed = 0.0f;
            m_FrameCount = 0;
        }
    }

    private void OnGUI()
    {
        //������ʾFPS
        GUI.Label(new Rect((Screen.width / 2) - 40, 0, 200, 200), "FPS: " + m_FPS, guiStyle);
    }
}
