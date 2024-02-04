using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCameraController : MonoBehaviour
{
    float mouseZoomSpeed = 50.0f;
    float touchZoomSpeed = 0.05f;
    float zoomMinBound = 10.0f;
    float zoomMaxBound = 50f;
    float dragSpeed = 0.005f; // �巡�� �̵� �ӵ� ����
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
                    // �巡�� ���⿡ ���� ī�޶� �̵��� ���Ʒ�/�¿�� ����
                    DragCamera(delta.y * dragSpeed, -delta.x * dragSpeed);
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
            if (Input.GetMouseButton(0)) 
            {
                Vector3 delta = new Vector3(-Input.GetAxis("Mouse Y"), 0, -Input.GetAxis("Mouse X"));
                DragCamera(delta.x * dragSpeed, delta.z * dragSpeed);
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

    void DragCamera(float deltaX, float deltaZ)
    {
        // ī�޶� �̵� ���� ����: x��� z�� ��� ����
        Vector3 move = new Vector3(deltaX, 0, deltaZ);
        cam.transform.Translate(move, Space.World);
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