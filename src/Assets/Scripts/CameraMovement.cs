using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private int mouseSens = 150;
    [SerializeField] private Transform body;

    float upDownLook = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

        upDownLook -= MouseY;
        upDownLook = Mathf.Clamp(upDownLook, -90f, 90f);
        transform.localRotation = Quaternion.Euler(upDownLook, 0f, 0f);

        body.Rotate(Vector2.up * MouseX * mouseSens * Time.deltaTime);
    }
    
}
