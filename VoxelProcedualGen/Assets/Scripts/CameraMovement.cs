using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private int mouseSens = 150;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        transform.localEulerAngles += GetMouseMove() * mouseSens * Time.deltaTime;
    }

    private Vector3 GetMouseMove()
    {
        return new Vector3 ( -Input.GetAxis("Mouse Y"), Mathf.Clamp(Input.GetAxis("Mouse X"), -90f, 90f), 0f );
    }
}
