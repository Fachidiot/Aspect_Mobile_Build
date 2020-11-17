using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    public Vector2 v_Standard_Resolution;
    public Camera cam_Camera;
    public bool b_IsLandScape;

    // 화면 비율
    private float m_fRate;

    private void Start()
    {
        m_fRate = v_Standard_Resolution.y / v_Standard_Resolution.x;
    }

    // Start와 Awake에서 화면값을 설정해주면 화면 회전이 끝나기 전에 실행됨.
    private void Update()
    {
        Setting();
    }

    void Setting()
    {
        int size = Screen.height - Screen.width;
        // size 양수 = 세로모드           size 음수 = 가로모드
        bool b_isScreenWrong = (size > 0 && b_IsLandScape) || (size < 0 && !b_IsLandScape);
        if (b_isScreenWrong)
            return;

        // 현재 비율 구하기
        float f_Mobile_Rate = (float)Screen.height / (float)Screen.width;

        // 위아래 비율이 늘어진 경우
        if (f_Mobile_Rate > m_fRate)
        {
            float h = m_fRate / f_Mobile_Rate;
            cam_Camera.rect = new Rect(0, (1 - h) / 2, 1, h);
        }

        // 좌우 비율이 늘어진 경우
        else
        {
            float w = f_Mobile_Rate / m_fRate;
            cam_Camera.rect = new Rect((1 - w) / 2, 0, w, 1);
        }

        this.enabled = false;
    }
}
