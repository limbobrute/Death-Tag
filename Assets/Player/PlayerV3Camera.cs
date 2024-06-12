using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerV3Camera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    Camera cam;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotate;
    float yRotate;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraInput();

        cam.transform.localRotation = Quaternion.Euler(xRotate, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotate, 0);
    }

    void CameraInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotate += mouseX * sensX * multiplier;
        xRotate -= mouseY * sensY * multiplier;

        xRotate = Mathf.Clamp(xRotate, -89f, 89f);
    }
}
