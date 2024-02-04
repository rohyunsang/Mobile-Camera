using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCameraController : MonoBehaviour
{
    float mouseZoomSpeed = 15.0f;
    float touchZoomSpeed = 0.1f;
    float zoomMinBound = 0.5f;
    float zoomMaxBound = 50f;
    float dragSpeed = 0.005f; // 드래그 이동 속도 조절
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchSupported && Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.deltaPosition;
                    DragCamera(new Vector3(-delta.x, -delta.y, 0) * dragSpeed);
                }
            }
            else if (Input.touchCount == 2)
            {
                // Pinch to zoom
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, touchZoomSpeed);
            }
        }
        else
        {
            if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼을 누르고 있을 때
            {
                Vector3 delta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
                DragCamera(delta * dragSpeed);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(scroll, mouseZoomSpeed);
        }

        ClampCameraFOV();
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        cam.fieldOfView += deltaMagnitudeDiff * speed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinBound, zoomMaxBound);
    }

    void DragCamera(Vector3 delta)
    {
        // 카메라 위치 업데이트
        cam.transform.Translate(delta, Space.World);
    }

    void ClampCameraFOV()
    {
        if (cam.fieldOfView < zoomMinBound)
        {
            cam.fieldOfView = zoomMinBound;
        }
        else if (cam.fieldOfView > zoomMaxBound)
        {
            cam.fieldOfView = zoomMaxBound;
        }
    }
}