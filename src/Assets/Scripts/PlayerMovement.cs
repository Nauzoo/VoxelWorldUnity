using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 15;

    private CharacterController charController;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        float upMove = Input.GetKey(KeyCode.Space) ? 1 : Input.GetKey(KeyCode.LeftShift) ? -1 : 0;        

        Vector3 direction = transform.right * xMove + transform.forward * zMove + transform.up * upMove;

        charController.Move(direction * speed * Time.deltaTime);
    }
}
