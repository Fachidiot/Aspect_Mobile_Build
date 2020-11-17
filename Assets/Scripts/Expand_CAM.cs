using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand_CAM : MonoBehaviour
{
    int touchCount;

    float m_OldTouchDistance;
    float m_NowTouchDistance;

    Vector3 oldPos;
    Vector3 panOrigin;

    //카메라의 이동, 줌인, 줌아웃 감도 변수
    public float moveSpeed = 2;

    float startTime = 0;

    //줌 인, 줌 아웃 되고 있을때에는 카메라를 움직일 수 없도록 trigger 선언
    bool trigger = false;
    public Camera expandCamera, mainCamera;

    void Awake()
    {
        startTime = Time.time;
    }

    void Update()
    {
        touchCount = Input.touchCount;

        if (touchCount == 2)
        {
            expandCamera.enabled = true;

            if (m_OldTouchDistance == 0)
                m_OldTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

            m_NowTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

            if (m_NowTouchDistance > m_OldTouchDistance)
            {
                trigger = true; //줌 인, 줌 아웃 되고 있을때에는 카메라를 움직일 수 없도록 trigger 선언
                ZoomIn();
            }
            else if (m_NowTouchDistance < m_OldTouchDistance)
            {
                trigger = true; //줌 인, 줌 아웃 되고 있을때에는 카메라를 움직일 수 없도록 trigger 선언
                ZoomOut();
            }

            m_OldTouchDistance = m_NowTouchDistance;
        }

        //컴퓨터에서 orthographic을 조절하고 ZoomOut 테스트용, 정상작동하면 지우자
        if (Input.GetKey(KeyCode.Q))
        {
            ZoomOut();
        }

        if (Input.GetMouseButtonDown(0))
        {
            oldPos = transform.position;
            panOrigin = expandCamera.ScreenToViewportPoint(Input.mousePosition);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (expandCamera.orthographicSize < 2 && !trigger) //카메라의 orthographicSize가 최대값보다 작고 trigger가 false일때 작동
            {
                Vector3 pos = expandCamera.ScreenToViewportPoint(Input.mousePosition) - panOrigin;

                //moveSpeed로 움직이는 속도 조절
                transform.position = oldPos + -pos * moveSpeed;

                //(카메라 사이즈가 줄어든 값) * 10
                float pixelRatio = (1 - expandCamera.orthographicSize) * 10;

                //36은 카메라가 줄어들때마다 늘어나는 카메라 x의 최대값
                if (expandCamera.transform.localPosition.x > (36 * pixelRatio) || expandCamera.transform.localPosition.x < -(36 * pixelRatio))
                {
                    Vector3 newCameraPosition = transform.localPosition;
                    newCameraPosition = new Vector3(Mathf.Clamp(newCameraPosition.x, -(36 * pixelRatio), (36 * pixelRatio)), newCameraPosition.y, 0);
                    transform.localPosition = newCameraPosition;
                }

                //64는 카메라가 줄어들때마다 늘어나는 카메라 y의 최대값
                if (expandCamera.transform.localPosition.y > (64 * pixelRatio) || expandCamera.transform.localPosition.y < -(64 * pixelRatio))
                {
                    Vector3 newCameraPosition = transform.localPosition;
                    newCameraPosition = new Vector3(newCameraPosition.x, Mathf.Clamp(newCameraPosition.y, -(64 * pixelRatio), (64 * pixelRatio)), 0);
                    transform.localPosition = newCameraPosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
            trigger = false;

        if (expandCamera.orthographicSize > 2)
            expandCamera.orthographicSize = 2;
        else if (expandCamera.orthographicSize < 0.5f)
            expandCamera.orthographicSize = 0.5f;

        if (expandCamera.orthographicSize == 2)
            expandCamera.enabled = false;
    }

    void ZoomOut()
    {
        if (expandCamera.orthographicSize < 2)
        {
            //moveSpeed로 줌 아웃 속도 조절
            expandCamera.orthographicSize += moveSpeed * Time.deltaTime;

            //줌아웃이 되면서 좌표를 초기값인 0,0,0으로 돌리는 부분
            float journeyLength = Vector3.Distance(expandCamera.transform.localPosition, mainCamera.transform.localPosition);
            float distCovered = (Time.time - startTime) * 10;
            float fracJourney = distCovered / journeyLength;

            Vector3 tempVector = expandCamera.transform.position;
            expandCamera.transform.localPosition = Vector3.Lerp(expandCamera.transform.position, Vector3.zero, fracJourney);
        }
    }

    void ZoomIn()
    {
        if (expandCamera.orthographicSize > 0.5f)
            //moveSpeed로 줌 인 속도 조절
            expandCamera.orthographicSize -= moveSpeed * Time.deltaTime;
    }
}
