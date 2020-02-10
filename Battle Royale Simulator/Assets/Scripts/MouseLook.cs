using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;

    float xRot;

    void Start()
    {
        mouseSensitivity = mouseSensitivity * Time.deltaTime;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRot -= mouseX;
        xRot = Mathf.Clamp(xRot, -90, -90);

        //transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
        //transform.Rotate(Vector3.down * mouseY);
    }
}
