using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float distance = 5;

    // For camera movement with mouse control
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 45f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    [SerializeField] private Vector2 framingOffset;

    private float rotationX;
    private float rotationY;
    private float invertXVal;
    private float invertYVal;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y") * invertXVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        rotationY += Input.GetAxis("Mouse X") * invertYVal * rotationSpeed;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPos = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPos - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }
}
