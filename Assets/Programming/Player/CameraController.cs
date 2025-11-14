using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3.0f;

    [SerializeField] private Transform targetToFollow;

    [SerializeField] private float distanceFromTarget = 3.0f;

    [SerializeField] private float smoothTime = 0.2f;

    [SerializeField] private Vector2 rotationLimit = new Vector2(-40, 40);

    private float rotY;
    private float rotX;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotY += mouseX;
        rotX += mouseY;

        rotX = Mathf.Clamp(rotX, rotationLimit.x, rotationLimit.y);

        Vector3 nextRotation = new Vector3(rotX, rotY);

        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

        transform.position = targetToFollow.position - transform.forward * distanceFromTarget;
    }
}
