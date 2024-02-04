using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCameraController : MonoBehaviour
{
    float mouseZoomSpeed = 50.0f;
    float touchZoomSpeed = 0.05f;
    float zoomMinBound = 10.0f;
    float zoomMaxBound = 50f;
    float dragSpeed = 0.005f; 
    float returnSpeed = 10f; // Camera Back To Origin Pos
    Camera cam;
    private bool returnToOrigin = false;   // 

    private Vector3 originCameraPosition;
    void Start()
    {
        cam = GetComponent<Camera>();
        originCameraPosition = cam.transform.position;
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

        if (returnToOrigin)
        {
            
            cam.transform.position = Vector3.Lerp(cam.transform.position, originCameraPosition, returnSpeed * Time.deltaTime);
            
            if (Vector3.Distance(cam.transform.position, originCameraPosition) < 0.01f)
            {
                cam.transform.position = originCameraPosition;
                returnToOrigin = false;
            }
        }
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        cam.fieldOfView += deltaMagnitudeDiff * speed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinBound, zoomMaxBound);

        returnToOrigin = cam.fieldOfView >= zoomMaxBound;
    }

    void DragCamera(float deltaX, float deltaZ)
    {
        // 카메라 이동 로직
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