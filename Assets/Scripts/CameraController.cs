using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float minDistance = 2;
    [SerializeField] private float distance = 5;
    [SerializeField] private float maxDistance = 6;

    // For camera movement with mouse control
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 45f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    [SerializeField] private Vector2 framingOffset;

    [field: SerializeField] public InputReader InputReader { get; private set; }

    private float rotationX;
    private float rotationY;
    private float invertXVal;
    private float invertYVal;

    private float zoomSensitivity = 0.1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        #region Old Input System
        // rotationX += Input.GetAxis("Mouse Y") * invertXVal * rotationSpeed;
        // rotationY += Input.GetAxis("Mouse X") * invertYVal * rotationSpeed;
        #endregion

        rotationX += InputReader.MousePos.y * invertXVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
       
        rotationY += InputReader.MousePos.x * invertYVal * rotationSpeed;

        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 focusPos = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPos - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;

        HandleZoom();
    }

    private void HandleZoom()
    {
        if (InputReader.MouseScrollY > 0)                 
            distance -= zoomSensitivity;        

        if (InputReader.MouseScrollY < 0)        
            distance += zoomSensitivity;        

        if (distance > maxDistance)
            distance = maxDistance;

        if (distance < minDistance)
            distance = minDistance;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
